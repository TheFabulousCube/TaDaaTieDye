/* Marquardt 'Cube' Snell
   www.tadaatiedye.com 
   email: cube@tadaatiedye.com 
   pagenav.js 
   This is a javascript defines
   page forward/page back links--*/
    
function pagenav(current, perpage, total)
{
	//document.write ("Hello world current: " + current + " perpage: " + perpage);
	var NUMBERSTODISPLAY = 5;
	var adjPageNumbers = NUMBERSTODISPLAY;
    var sender = window.location.pathname;
    var LowerBound = ((current - (adjPageNumbers / 2 * perpage)) / perpage);
    var middle = 0;
    var upper = ((current + (((adjPageNumbers / 2) + 1) * perpage)) / perpage);
    perpageList = [];
   for (var i = 5; i <= total / 2; i = i + 5) {perpageList.push(i);}
   current = Math.floor(current);
	adjPageNumbers = (NUMBERSTODISPLAY < ((total - 1) / perpage) + 1) ? NUMBERSTODISPLAY : (((total - 1) / perpage) + 1);
   // fixes the mis match of changing perpage to an odd multiple of current
   while (current%perpage != 0 && current >= 0)
	{
	current -= 1;
	}

   document.write ("<ul class='pager'>");
	   if (LowerBound < 0)
	{
		LowerBound = 0;
		UpperBound = LowerBound + adjPageNumbers;
	}
	if (UpperBound > (((total - 1) / perpage) + 1))
	{
		UpperBound = (((total - 1) / perpage) + 1);
		LowerBound = UpperBound - adjPageNumbers;   
	}

	if (total > perpage) // if there's not enough items to page, just display the page
	{
		   if (current >= perpage && perpage * adjPageNumbers < total)
		   {
			   document.write ("<li><a href=" + sender + "?current=0" + "&perpage=" + perpage + "><< </a>");
		   }
		   else
		   {
			   document.write ("<li></li>");	   
		   }
		   
		    // BE VERY carefull!!! The page numbers are array-style, they are 0-9, the display must be 1-10
            for (page = LowerBound; page < (UpperBound); page++)
            {
                if (total-((page)*perpage)>0 && !(current==(page)*perpage)) 
                    {
						document.write ("<li><a href=" + sender + "?current=" + (page * perpage) + "&perpage=" + perpage + "> " + (page + 1) + " </a></li>");
                         
                    }
                    else
                    {
                        document.write ("<li>" + (page + 1) + "</li>");    
                    }
            }
			
			if (current < total - perpage && perpage * adjPageNumbers < total)
			{
				document.write ("<li><a href=" + sender + "?current=" + (total-perpage) + "&perpage=" + perpage + "> >> </a></li>");
			}
			else
			{
				document.write ("<li></li>");  
			}	   

			document.write ('<form action="">');
			document.write ('<select name="perpage" onchange="this.form.submit()">');
			// outputs values from 5 to 1/2 of the total, selecting the perpage number
			for (i=5; i <= (total/2); i=i+5)
			{
				document.write ('<option value="' + i + '"');
				if (perpage == i) 
				{
					document.write (' selected ');
				}
				document.write ('>' + i + '</option>');
			}
			
		   document.write ('<input type="hidden" name="current" value=' + current + '>');
		   document.write ('<input type="hidden" name="total" value=' + total + '>');
				document.write ('</select>');
				document.write ('</form>');
	}			
		   document.write ("</li>");
   document.write ("<ul>");   
 
}
   