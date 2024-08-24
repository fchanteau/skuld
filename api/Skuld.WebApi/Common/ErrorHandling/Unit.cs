namespace Skuld.WebApi.Common.ErrorHandling;

public class Unit
{
	private Unit () { }
	public static Unit Instance => new ();
}
