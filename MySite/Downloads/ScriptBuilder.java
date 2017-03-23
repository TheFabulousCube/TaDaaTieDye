public class ScriptBuilder {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		List<String> file = new ArrayList<String>();
		int j  = 75; // start @ 0:00 (midnight)*** Do NOT use leading zeros,this makes 'j' an OCTAL
		String temp;
		for (int i = 0; i <=24; i++, j += 15) // j+= 15 for CW, j-=15 for CCW
		{
      
         j = (j>360) ? j-360 : j; // clockwise out of bounds adjustment
         j = (j < 0) ? j+360 : j; // counterclockwise out of bounds adjustment
			System.out.println("case " + i + ":");
			System.out.println("    document.write(\"<link rel='stylesheet' href='../stylesheets/colors/color" + pad(j) + "degrees.css' type='text/css'>\");");
			System.out.println("    break;");
					
					//"C:\WebServer\tadaatiedye\stylesheets\colors"
		}
	}
	static String pad(int i)
	{
      String raw;
      raw = Integer.toString(i);
		if (i < 10 ) return ("00" + raw);
		if (i < 100) return ("0" + raw);
		return raw;
		
	}

}
