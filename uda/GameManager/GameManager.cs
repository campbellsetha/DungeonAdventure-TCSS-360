using Godot;
using System;
using System.Reflection.Metadata.Ecma335;
using UDA.Model;

public partial class GameManager : Node
{
    //TODO: attach these save and load scripts to buttons and test if the save function works and what needs to change
    private Player _myPlayerInstance;

    public override void _Process(double delta)
    {
        
        //Test to take damage and check health
        if (Input.IsActionJustPressed("space"))
        {
            //var temp = _playerInstance.GetNode<PlayerClass>("Player/PlayerClass");
            //GD.Print(temp.MY_CLASS.HitPoints);
        }

        if (Input.IsActionJustPressed("save"))
        {
            OnSaveGame();
            GD.Print("GameSaved");
        }

        if (Input.IsActionJustPressed("load"))
        {
            OnLoadGame();
            GD.Print("GameLoaded");
        }
        if (Input.IsActionJustPressed("QueryPlayerInfo"))
        {
            GD.Print(_myPlayerInstance.MyClass.ToString());
        }
        
    }

    public override void _Ready()
    {
        
        //Grab the player node
        _myPlayerInstance = GetNode<Player>("Player");
        //Now we have to check if the player already has a class and name, this is important for loading from state
        if (_myPlayerInstance.MyClass == null && _myPlayerInstance.MyName == null)
        {
            //We want to set its name first, because attempting to set it's class will cause issues otherwise
            _myPlayerInstance.MyName = "Gribble jenkins";
            //Now we are going to set its class to an appropiate choice
            _myPlayerInstance.SetClass("Warrior");
        }
    }

    public void OnSaveGame()
    {
        //Most of this is ripped right of off the docs, and will likely need to be tweaked
        //@See https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html
        var saveFile = FileAccess.Open("user://saveGame.save", FileAccess.ModeFlags.Write);
        
        //TODO: Change this to manually perform the save on every object we want to save
        var saveNodes = GetTree().GetNodesInGroup("Player");
        
        //Likely need to load the parent node first, might need to manually adjust this 
        //So that it loads the player, and then player class
        //World, and then room -> etc.
        foreach (Node saveNode in saveNodes)
        {
            if (string.IsNullOrEmpty(saveNode.SceneFilePath))
            {
                GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
                continue;
            }

            // Check the node has a save function.
            if (!saveNode.HasMethod("Save"))
            {
                GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
                continue;
            }

            // Call the node's save function.
            var nodeData = saveNode.Call("Save");

            // Json provides a static method to serialized JSON string.
            var jsonString = Json.Stringify(nodeData);

            // Store the save dictionary as a new line in the save file.
            saveFile.StoreLine(jsonString);
        }
    }

    //Because some of the objects are children of other persistent objects
    //They need to be loaded in stages
    //With top level(Parent) objects being loaded first
    public void OnLoadGame()
    {
        if (!FileAccess.FileExists("user://saveGame.save"))
        {
            //TODO: Maybe throw up a splash screen for lack of a save file?
            //In the interem we can just have this do nothing
            return;
        }

        //Have to revert game state to avoid the cloning of objects
        //Currently this is done by 
        var nodesToLoad = GetTree().GetNodesInGroup("Player");
        foreach (Node loadingNode in nodesToLoad)
        {
            loadingNode.QueueFree();
            
        }

        //Load the file line by line and process the dictionary
        using (var saveFile = FileAccess.Open("user://saveGame.save", FileAccess.ModeFlags.Read)) {

            while (saveFile.GetPosition() < saveFile.GetLength())
            {
                var jsonString = saveFile.GetLine();

                //helper class to interact with json
                var json = new Json();
                var parseResult = json.Parse(jsonString);

                // the return of .Ok signals that an error has occured
                if (parseResult != Error.Ok)
                {
                    GD.Print(
                        $"JSON Parse Error:{json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                }

                //Get data for JSON object
                //This pulls the next line in the json file and reads it as a 
                //dictionary provided by Godot's API
                var nodeData =
                    new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

                //Create objects and it to the tree
                //TODO: create a dictionary for the playerMove script on 2dBody node
                string path = nodeData["FileName"].ToString();
                
                var newObjectScene = GD.Load<PackedScene>(nodeData["FileName"].ToString());
                var newObject = newObjectScene.Instantiate<Node>();

                //This really only works for root level nodes
                //Note: If node already has a parent, this method will fail. Use RemoveChild(Node) first to remove node from its current parent. For example:
                //TODO: Figure out how to do this for specific nodes 
                GetNode(nodeData["Parent"].ToString()).AddChild(newObject);

                //This sets the default position for the object
                //TODO: Determine if this is based on the nodes last position
                //Or if this needs to be saved and recalled as well
                //I Believe the position will need to be manually configured.
                //If a node does not have a position then setting this could break this method
                //I WILL NEED TO SET ALL OF THESE WHEN LOADING IN AN OBJECT
                newObject.Set(Node2D.PropertyName.Position,
                    new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));

                //sets the remaining variables
                //Looks like it checks each value in the nodes json data for specific types, and if they do not exist it 
                //sets them manually
                //This might need to be set up specifically for each instance.
                foreach (var (key, value) in nodeData)
                {
                    if (key == "FileName" || key == "Parent" || key == "PosX" || key == "PosY")
                    {
                        continue;
                    }
                    newObject.Set(key, value);
                }
            }
        }
    }
}
