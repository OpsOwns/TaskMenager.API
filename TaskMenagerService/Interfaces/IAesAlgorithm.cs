namespace TaskMenagerService.Interfaces
{
	public interface IAesAlgorithm
	{
		string Decrypt(string text);
		string Encrypt(string text);
	}
}