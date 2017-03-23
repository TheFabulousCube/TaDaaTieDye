public class colorbuilder {
	public static void main(String[] args) throws IOException {
		String fileName = "";
		String name = "";
		String color = "";
		Scanner scan = new Scanner(new File("C:\\WebServer\\tadaatiedye\\stylesheets\\colors.txt"));
		 
		List<String> colorLines = new ArrayList<String>();
		List<String> styleLines = Files.readAllLines(Paths.get("C:\\WebServer\\tadaatiedye\\stylesheets\\color_template.css"));
		
		for (int i = 0; i < styleLines.size(); i++)
		{
			colorLines.add(styleLines.get(i));
		}
		
		while (scan.hasNextLine())	{
			String temp = scan.next();
			if (temp.contains("degrees"))
			{
				fileName = "C:\\WebServer\\tadaatiedye\\stylesheets\\colors\\" + temp +".css";
				System.out.println("Starting: " + fileName);
				
			} else if (temp.equals("end"))
			{
				Files.write(Paths.get(fileName), colorLines, StandardOpenOption.CREATE);
				System.out.println("writing to file . . . " + fileName);
				for (int i = 0; i < styleLines.size(); i++)
				{
					colorLines.set(i, styleLines.get(i));
				}

			} else
			{
				name=temp.replace(":", ";");
				color = scan.next();
				System.out.println("the name is: " + name + " and the color is: " + color);
//			names.add(name);
//			colors.add(color);
			for(int i = 0; i < colorLines.size(); i++)
				{
					if (colorLines.get(i).contains(name))
						{
							colorLines.set(i, colorLines.get(i).replace(name, color));
							System.out.println("relpaced " + colorLines.get(i));
							System.out.println(colorLines.get(i).replace(name, color));
						}  //else if (fileName.equals("C:\\WebServer\\tadaatiedye\\stylesheets\\colors\\color015degrees.css"))System.out.println(colorLines.get(i));
				}
			}
		}

		scan.close();
	}

}
