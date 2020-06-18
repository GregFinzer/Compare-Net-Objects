using System;

public class PersonWithNotImplementedProperty
{
	public DateTime DateCreated { get; internal set; }
	public string Name
	{
		get
		{
			throw new NotImplementedException("Name should be ignored but this exception is thrown.");
		}
	}
}