import os

def find_files_without_comment():
    files_without_comment = []

    for root, dirs, files in os.walk('SunamoDevCode'):
        # Skip obj and bin directories
        dirs[:] = [d for d in dirs if d not in ['obj', 'bin']]

        for file in files:
            if file.endswith('.cs'):
                filepath = os.path.join(root, file)
                try:
                    with open(filepath, 'r', encoding='utf-8', errors='ignore') as f:
                        first_line = f.readline().strip()
                        if first_line != '// variables names: ok':
                            files_without_comment.append(filepath)
                except Exception as e:
                    print(f"Error reading {filepath}: {e}")

    return files_without_comment

if __name__ == '__main__':
    files = find_files_without_comment()
    for f in files:
        print(f)
    print(f"\nTotal: {len(files)} files")
