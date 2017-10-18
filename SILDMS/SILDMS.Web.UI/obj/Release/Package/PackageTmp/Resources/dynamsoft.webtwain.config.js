//
// Dynamsoft JavaScript Library for Basic Initiation of Dynamic Web TWAIN
// More info on DWT: http://www.dynamsoft.com/Products/WebTWAIN_Overview.aspx
//
// Copyright 2015, Dynamsoft Corporation 
// Author: Dynamsoft Team
// Version: 11.1
//
/// <reference path="dynamsoft.webtwain.initiate.js" />
var Dynamsoft = Dynamsoft || { WebTwainEnv: {} };

Dynamsoft.WebTwainEnv.AutoLoad = true;
///
//Dynamsoft.WebTwainEnv.Containers = [{ ContainerId: 'dwtcontrolContainer', Width: 100, Height: 350 }];

Dynamsoft.WebTwainEnv.Containers = [{ ContainerId: 'dwtHorizontalThumbnil', Width: 100+'%', Height: 200 },
{ ContainerId: 'dwtVerticalThumbnil', Width: 150, Height: 500 },
{ ContainerId: 'dwtLargeViewer', Width: 755, Height: 500 },
{ ContainerId: 'dwtQuickViewer', Width: 80 +'%', Height: 500 }];

///
Dynamsoft.WebTwainEnv.ProductKey = '5FD89B2563BEAAC29FEC6ADB0D5CC0591365ADDF05DFAF3E69E21E9F3CD65B651365ADDF05DFAF3E7F407E32B34243761365ADDF05DFAF3E5FD32DF6C1AD0CEE1365ADDF05DFAF3EADFA5DD3B0170F491365ADDF05DFAF3E6B1ACC8FC7E635CD1365ADDF05DFAF3E29CE75AF14446EA61365ADDF05DFAF3EFFE903C60DF686971365ADDF05DFAF3E2C339AF0A4443C9880000000';
///
Dynamsoft.WebTwainEnv.Trial = true;
///
Dynamsoft.WebTwainEnv.ActiveXInstallWithCAB = false;
///
Dynamsoft.WebTwainEnv.Debug = false; // only for debugger output
///
 Dynamsoft.WebTwainEnv.ResourcesPath = '../Resources';

/// All callbacks are defined in the dynamsoft.webtwain.install.js file, you can customize them.

 Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', function(){
 		// webtwain has been inited
 });

