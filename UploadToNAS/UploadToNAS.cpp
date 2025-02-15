// UploadToNAS.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include <iostream>
#include <fstream>
#include <filesystem>
#include <chrono>
#include <iomanip>
#include <sstream>

namespace fs = std::filesystem;
using namespace std::chrono;

#ifdef _WIN32
#define NOMINMAX
#include <windows.h>
#include <locale>
#include <codecvt>
#endif

// Windowsのための文字列変換
#ifdef _WIN32
std::wstring string_to_wstring(const std::string& str) {
    int size_needed = MultiByteToWideChar(CP_UTF8, 0, str.c_str(), -1, nullptr, 0);
    std::wstring wstr(size_needed, 0);
    MultiByteToWideChar(CP_UTF8, 0, str.c_str(), -1, &wstr[0], size_needed);
    return wstr;
}
#endif

// `YYYY-MM-DD HH:MM:SS` 形式の現在時刻を取得
std::string get_current_time_string() {
    auto now = system_clock::to_time_t(system_clock::now());
    std::tm tm;
#ifdef _WIN32
    localtime_s(&tm, &now);  // Windows
#else
    localtime_r(&now, &tm);  // Linux / macOS
#endif

    std::ostringstream oss;
    oss << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
    return oss.str();
}

// `YYYY-MM-DD HH:MM:SS` の文字列を `fs::file_time_type` に変換
fs::file_time_type to_file_time(const std::string& timestamp) {
    std::tm tm = {};
    std::istringstream ss(timestamp);
    ss >> std::get_time(&tm, "%Y-%m-%d %H:%M:%S");
    if (ss.fail()) {
        return fs::file_time_type::min();  // 変換失敗時は最小値
    }
    auto time_c = std::mktime(&tm);
    return fs::file_time_type{seconds{time_c}};
}

// `YYYY/yyyymmddhhmm` 形式のフォルダ名を作成
std::string get_timestamp_folder() {
    auto now = system_clock::to_time_t(system_clock::now());
    std::tm tm;
#ifdef _WIN32
    localtime_s(&tm, &now);
#else
    localtime_r(&now, &tm);
#endif

    std::ostringstream oss;
    oss << std::put_time(&tm, "%Y/%Y%m%d%H%M");
    return oss.str();
}

// `last_run.txt` から前回の実行時間を取得
fs::file_time_type load_last_run_time(const fs::path& filePath) {
    if (!fs::exists(filePath)) {
        return fs::file_time_type::min();  // ファイルがない場合は最小値
    }
    std::ifstream file(filePath);
    std::string timestamp;
    if (file.is_open() && std::getline(file, timestamp)) {
        return to_file_time(timestamp);
    }
    return fs::file_time_type::min();
}

// 現在時刻を `last_run.txt` に保存
void save_current_time(const fs::path& filePath) {
    std::ofstream file(filePath);
    if (file.is_open()) {
        file << get_current_time_string();
    }
}

void copy_recent_files(const fs::path& srcDir, const fs::path& baseDestDir, const fs::path& lastRunFile) {
    // `last_run.txt` から前回の実行時間を取得
    fs::file_time_type cutoff_time = load_last_run_time(lastRunFile);

    // `last_run.txt` がない or 壊れている場合、現在時刻を記録して終了（初回起動処理）
    if (cutoff_time == fs::file_time_type::min()) {
        std::cout << "初回起動です。現在の時刻を記録し、ファイルコピーは行いません。" << std::endl;
        save_current_time(lastRunFile);
        return;
    }

    // `YYYY/yyyymmddhhmm` のフォルダを作成
    std::string timestamp_folder = get_timestamp_folder();
    fs::path destDir = baseDestDir / timestamp_folder;

    if (!fs::exists(destDir)) {
        fs::create_directories(destDir);
    }

    bool copied = false;

    for (const auto& entry : fs::recursive_directory_iterator(srcDir)) {
        if (fs::is_regular_file(entry)) {
            fs::file_time_type last_write_time = fs::last_write_time(entry);

            if (last_write_time > cutoff_time) {  // 前回起動後に変更されたファイル
                fs::path relativePath = fs::relative(entry.path(), srcDir);
                fs::path destPath = destDir / relativePath;

                fs::create_directories(destPath.parent_path());
                fs::copy_file(entry, destPath, fs::copy_options::overwrite_existing);
                copied = true;

                std::cout << "Copied: " << entry.path() << " -> " << destPath << std::endl;
            }
        }
    }

    if (copied) {
        save_current_time(lastRunFile);  // コピーが発生した場合のみ `last_run.txt` を更新
    }
    else {
        std::cout << "新しいファイルはありません。" << std::endl;
    }
}

int main(int argc, char* argv[]) {
    std::wcout.imbue(std::locale("Japanese"));
    if (argc != 4) {
        std::cerr << "使い方: " << argv[0] << " <コピー元フォルダ> <コピー先フォルダ> <last_run.txt のパス>\n";
        return 1;
    }

#ifdef _WIN32
    fs::path sourceDir = string_to_wstring(argv[1]);
    fs::path destinationDir = string_to_wstring(argv[2]);
    fs::path lastRunFile = string_to_wstring(argv[3]);
#else
    fs::path sourceDir = argv[1];
    fs::path destinationDir = argv[2];
    fs::path lastRunFile = argv[3];
#endif

    std::wcout << L"コピー元: " << sourceDir.wstring() << std::endl;
    std::wcout << L"コピー先: " << destinationDir.wstring() << std::endl;

    // 入力ディレクトリの存在確認
    if (!fs::exists(sourceDir) || !fs::is_directory(sourceDir)) {
        std::cerr << "エラー: コピー元フォルダが存在しません。\n";
        return 1;
    }

    std::cout << "test" << std::endl;

    // 出力ディレクトリの存在確認（なければ作成）
    if (!fs::exists(destinationDir)) {
        fs::create_directories(destinationDir);
    }

    copy_recent_files(sourceDir, destinationDir, lastRunFile);

    return 0;
}


// プログラムの実行: Ctrl + F5 または [デバッグ] > [デバッグなしで開始] メニュー
// プログラムのデバッグ: F5 または [デバッグ] > [デバッグの開始] メニュー

// 作業を開始するためのヒント: 
//    1. ソリューション エクスプローラー ウィンドウを使用してファイルを追加/管理します 
//   2. チーム エクスプローラー ウィンドウを使用してソース管理に接続します
//   3. 出力ウィンドウを使用して、ビルド出力とその他のメッセージを表示します
//   4. エラー一覧ウィンドウを使用してエラーを表示します
//   5. [プロジェクト] > [新しい項目の追加] と移動して新しいコード ファイルを作成するか、[プロジェクト] > [既存の項目の追加] と移動して既存のコード ファイルをプロジェクトに追加します
//   6. 後ほどこのプロジェクトを再び開く場合、[ファイル] > [開く] > [プロジェクト] と移動して .sln ファイルを選択します
