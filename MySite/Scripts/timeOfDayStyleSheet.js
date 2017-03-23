/* Marquardt 'Cube' Snell
   www.tadaatiedye.com 
   email: cube@tadaatiedye.com 
   timeOfDayStyleSheet.js 
   This is a javascript that calculates the time of day and assigns a 
   different colored style sheet for different times of the day. --*/

 
function getStylesheet() 
{

  var currentTime = new Date().getHours()


  switch (currentTime) {

case 0:
    document.write("<link rel='stylesheet' href='/Content/colors/color075degrees.css' type='text/css'>");
    break;
case 1:
    document.write("<link rel='stylesheet' href='/Content/colors/color090degrees.css' type='text/css'>");
    break;
case 2:
    document.write("<link rel='stylesheet' href='/Content/colors/color105degrees.css' type='text/css'>");
    break;
case 3:
    document.write("<link rel='stylesheet' href='/Content/colors/color120degrees.css' type='text/css'>");
    break;
case 4:
    document.write("<link rel='stylesheet' href='/Content/colors/color135degrees.css' type='text/css'>");
    break;
case 5:
    document.write("<link rel='stylesheet' href='/Content/colors/color150degrees.css' type='text/css'>");
    break;
case 6:
    document.write("<link rel='stylesheet' href='/Content/colors/color165degrees.css' type='text/css'>");
    break;
case 7:
    document.write("<link rel='stylesheet' href='/Content/colors/color180degrees.css' type='text/css'>");
    break;
case 8:
    document.write("<link rel='stylesheet' href='/Content/colors/color195degrees.css' type='text/css'>");
    break;
case 9:
    document.write("<link rel='stylesheet' href='/Content/colors/color210degrees.css' type='text/css'>");
    break;
case 10:
    document.write("<link rel='stylesheet' href='/Content/colors/color225degrees.css' type='text/css'>");
    break;
case 11:
    document.write("<link rel='stylesheet' href='/Content/colors/color240degrees.css' type='text/css'>");
    break;
case 12:
    document.write("<link rel='stylesheet' href='/Content/colors/color255degrees.css' type='text/css'>");
    break;
case 13:
    document.write("<link rel='stylesheet' href='/Content/colors/color270degrees.css' type='text/css'>");
    break;
case 14:
    document.write("<link rel='stylesheet' href='/Content/colors/color285degrees.css' type='text/css'>");
    break;
case 15:
    document.write("<link rel='stylesheet' href='/Content/colors/color300degrees.css' type='text/css'>");
    break;
case 16:
    document.write("<link rel='stylesheet' href='/Content/colors/color315degrees.css' type='text/css'>");
    break;
case 17:
    document.write("<link rel='stylesheet' href='/Content/colors/color330degrees.css' type='text/css'>");
    break;
case 18:
    document.write("<link rel='stylesheet' href='/Content/colors/color345degrees.css' type='text/css'>");
    break;
case 19:
    document.write("<link rel='stylesheet' href='/Content/colors/color360degrees.css' type='text/css'>");
    break;
case 20:
    document.write("<link rel='stylesheet' href='/Content/colors/color015degrees.css' type='text/css'>");
    break;
case 21:
    document.write("<link rel='stylesheet' href='/Content/colors/color030degrees.css' type='text/css'>");
    break;
case 22:
    document.write("<link rel='stylesheet' href='/Content/colors/color045degrees.css' type='text/css'>");
    break;
case 23:
    document.write("<link rel='stylesheet' href='/Content/colors/color060degrees.css' type='text/css'>");
    break;
case 24:
    document.write("<link rel='stylesheet' href='/Content/colors/color075degrees.css' type='text/css'>");
    break;

default:
	document.write("<link rel='stylesheet' href='stylesheets/colors/color000degrees.css' type='text/css'>")
  }
}

getStylesheet();