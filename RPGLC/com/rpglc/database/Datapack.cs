using com.rpglc.core;
using com.rpglc.database.TO;
using com.rpglc.json;

namespace com.rpglc.database;

public class Datapack {
    private readonly string datapackNamespace;

    public Datapack(string path) {
        datapackNamespace = Path.GetFileName(Path.GetDirectoryName(path));

        LoadClasses(Path.Combine(path, "classes"));
        LoadRaces(Path.Combine(path, "races"));
        LoadEffectTemplates(Path.Combine(path, "effects"));
        LoadEventTemplates(Path.Combine(path, "events"));
        LoadItemTemplates(Path.Combine(path, "items"));
        LoadObjectTemplates(Path.Combine(path, "objects"));
        LoadResourceTemplates(Path.Combine(path, "resources"));
    }

    private void LoadClasses(string path) {
        LoadClasses("", path);
    }

    private void LoadClasses(string classNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            LoadClasses($"{classNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject classJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{classNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            RPGL.AddRPGLClass(new RPGLClass(classJson.AsDict()));
        }
    }

    private void LoadRaces(string path) {
        LoadRaces("", path);
    }

    private void LoadRaces(string raceNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            LoadRaces($"{raceNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }

        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject raceJson = new JsonObject().LoadFromFile($"{filePath}")
                .PutString("datapack_id", $"{datapackNamespace}:{raceNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}");

            DBManager.InsertRPGLRace(raceJson);
        }
    }

    private void LoadEffectTemplates(string path) {
        LoadEffectTemplates("", path);
    }

    private void LoadEffectTemplates(string templateNameBase, string path) {
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

    private void LoadEventTemplates(string templateNameBase, string path) {
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

    private void LoadItemTemplates(string templateNameBase, string path) {
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

    private void LoadObjectTemplates(string templateNameBase, string path) {
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

    private void LoadResourceTemplates(string templateNameBase, string path) {
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
