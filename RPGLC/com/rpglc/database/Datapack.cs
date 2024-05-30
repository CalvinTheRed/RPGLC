using com.rpglc.json;

namespace com.rpglc.database;

internal class Datapack {
    private readonly string datapackNamespace;

    public Datapack(string path) {
        datapackNamespace = Path.GetFileName(Path.GetDirectoryName(path));
        
        LoadEffectTemplates(Path.Combine(path, "effects"));
        LoadEventTemplates(Path.Combine(path, "events"));
        LoadItemTemplates(Path.Combine(path, "items"));
        LoadObjectTemplates(Path.Combine(path, "objects"));
        LoadResourceTemplates(Path.Combine(path, "resources"));
    }

    private void LoadEffectTemplates(string path) {
        LoadEffectTemplates("", path);
    }

    internal void LoadEffectTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            LoadEffectTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLEffectTemplate(templateJson);
        }
    }

    private void LoadEventTemplates(string path) {
        LoadEventTemplates("", path);
    }

    internal void LoadEventTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            this.LoadEventTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLEventTemplate(templateJson);
        }
    }

    private void LoadItemTemplates(string path) {
        LoadItemTemplates("", path);
    }

    internal void LoadItemTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            this.LoadItemTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLItemTemplate(templateJson);
        }
    }

    private void LoadObjectTemplates(string path) {
        LoadObjectTemplates("", path);
    }

    internal void LoadObjectTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            this.LoadObjectTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLObjectTemplate(templateJson);
        }
    }

    private void LoadResourceTemplates(string path) {
        LoadResourceTemplates("", path);
    }

    internal void LoadResourceTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            this.LoadResourceTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLResourceTemplate(templateJson);
        }
    }

};
