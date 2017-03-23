/* Marquardt 'Cube' Snell
   www.tadaatiedye.com 
   email: cube@tadaatiedye.com 
   writeTimeAndDate.js 
   This is a javascript that parses the time 
   of day and writes it to the screen --*/
function pad(n)
{
	return (n<10) ? ("0" + n) : n;
}

function writeTimeAndDate() 
{
		var d=new Date()
		var weekday=new Array("Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday")
		var monthname=new Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec")
		document.write(weekday[d.getDay()] + " ")
		document.write(monthname[d.getMonth()] + " ")
		document.write(d.getDate() + ", ")
		document.write(d.getFullYear() + "  ")
		document.write(d.getHours()%12 + ":")
		document.write(pad(d.getMinutes()))


}
writeTimeAndDate();