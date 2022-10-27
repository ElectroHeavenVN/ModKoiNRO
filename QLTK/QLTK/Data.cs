namespace QLTK
{
	public class Data
	{
		public string username { get; set; }

		public string password { get; set; }

		public int server { get; set; }

		public string note { get; set; }

		public string getData()
		{
			return username + "|" + server + "|" + password;
		}
	}
}
