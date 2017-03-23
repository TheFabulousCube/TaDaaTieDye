import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.List;
import java.util.regex.*;
import java.io.*;

public class FileCleaner {
	
	
	public static void main(String[] args) throws IOException
	{
		 
		List<String> styleLines = Files.readAllLines(Paths.get("C:\\WebServer\\tadaatiedye\\stylesheets\\colors.bak"));
		int emptyCount = 0;
		int commentCount = 0;
		for(int i = 0; i < styleLines.size(); i++)
		{
			System.out.println(styleLines.get(i));
         if (styleLines.get(i).isEmpty())
            {
               styleLines.remove(i);
               emptyCount++;
            }
			if (styleLines.get(i).contains("Main"))
					{
						styleLines.set(i, styleLines.get(i).replaceAll("[/][*].+", ""));
// 						styleLines.set(i, styleLines.get(i).replace("/* Main Secondary color (1) */", ""));
// 						styleLines.set(i, styleLines.get(i).replace("/* Main Secondary color (2) */", ""));
// 						styleLines.set(i, styleLines.get(i).replace("/* Main Complement color */", ""));
						commentCount++;
						System.out.println("edited line: " + styleLines.get(i));
					}
			
		}System.out.println("removed " + emptyCount + " blank lines and " + commentCount + " comments.");
		Files.write(Paths.get("C:\\WebServer\\tadaatiedye\\stylesheets\\colors.new"), styleLines, StandardOpenOption.CREATE);
	} 
}
