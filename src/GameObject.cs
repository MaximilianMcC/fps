// TODO: See if something like this can work for static classes
public abstract class GameObject
{
	// TODO: In start add the object to a list of objects to be automatically updated and stuff
	public abstract void Start();
	public abstract void Update();
	public abstract void Render();
	public abstract void CleanUp();
}