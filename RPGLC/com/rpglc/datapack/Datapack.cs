using com.rpglc.database;
using com.rpglc.json;

namespace com.rpglc.datapack;

public class Datapack {

    public string datapackNamespace;

    public static void LoadDatapacks(string path) {
        try {
            foreach (string dirPath in Directory.GetDirectories(path)) {
                new Datapack(dirPath);
            }
        } catch (Exception e) {
            Console.WriteLine("Error reading datapack.");
            Console.WriteLine(e.ToString());
        }
    }

    public Datapack(string path) {
        this.datapackNamespace = new DirectoryInfo(path).Name;

        foreach(string dirPath in Directory.GetDirectories(path)) {
            string dirPathName = new DirectoryInfo(dirPath).Name;
            switch (dirPathName) {
                case "effects":
                    this.LoadEffectTemplates(dirPath);
                    break;
                case "events":
                    //this.LoadEventTemplates(dirPath);
                    break;
                case "items":
                    //this.LoadItemTemplates(dirPath);
                    break;
                case "objects":
                    //this.LoadObjectTemplates(dirPath);
                    break;
                case "resources":
                    //this.LoadResourceTemplates(dirPath);
                    break;
                case "classes":
                    //this.LoadClasses(dirPath);
                    break;
                case "races":
                    //this.LoadRaces(dirPath);
                    break;
                default:
                    //Console.WriteLine($"Error: datapack directory path name not recognized: {dirPathName}");
                    break;
            }
        }
    }

    public string GetDatapackNamespace() {
        return this.datapackNamespace;
    }

    private void LoadEffectTemplates(string path) {
        this.LoadEffectTemplates("", path);
    }

    internal void LoadEffectTemplates(string templateNameBase, string path) {
        // resurse over nested directories
        foreach (string dirPath in Directory.GetDirectories(path)) {
            this.LoadEffectTemplates($"{templateNameBase}{new DirectoryInfo(dirPath).Name}{Path.DirectorySeparatorChar}", dirPath);
        }
        
        // load files
        foreach (string filePath in Directory.GetFiles(path)) {
            string filePathName = Path.GetFileName(filePath);
            filePathName = filePathName[..filePathName.IndexOf('.')];

            JsonObject templateJson = new JsonObject().LoadFromFile($"{filePath}");
            DBQuery.InsertEffectTemplate(
                this.datapackNamespace,
                $"{templateNameBase.Replace(Path.DirectorySeparatorChar, '/')}{filePathName}",
                templateJson.GetString("name"),
                templateJson.GetString("description"),
                (templateJson.GetJsonObject("subevent_filters") ?? new JsonObject()).ToString()
            );
        }
    }
}
