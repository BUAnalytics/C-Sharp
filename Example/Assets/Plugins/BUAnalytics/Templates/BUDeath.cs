namespace BUAnalytics{

	public struct BUDeathLocation{
		public int X;
		public int Y;
		public int? Z;

		public BUDeathLocation(int x, int y){
			X = x;
			Y = y;
			Z = null;
		}

		public BUDeathLocation(int x, int y, int z){
			X = x;
			Y = y;
			Z = z;
		}
	}

	public class BUDeath: BUTemplate{

		public string Collection;

		public BUDeathLocation? Location;

		public BUDeath(BUDeathLocation location, string collection) {
			Location = location;
			Collection = collection;
		}

		public void Upload(){

			//Add optional fields
			if (Location != null){ Add("location", Location); }

			base.Upload(Collection ?? Collection);
		}
	}

}