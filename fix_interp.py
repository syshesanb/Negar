import re
import glob

for path in glob.glob('**/*.vb', recursive=True):
    with open(path, 'r', encoding='utf-8', errors='ignore') as f:
        text = f.read()
    
    def repl(m):
        content = m.group(1)
        parts = re.split(r'\{([^}]+)\}', content)
        out = []
        for idx, part in enumerate(parts):
            if idx % 2 == 0:
                if part:
                    out.append('"' + part + '"')
            else:
                out.append(part)
        return ' & '.join(out)

    new_text = re.sub(r'\$"([^"]*)"', repl, text)
    if new_text != text:
        with open(path, 'w', encoding='utf-8') as f:
            f.write(new_text)
        print('Fixed:', path)
