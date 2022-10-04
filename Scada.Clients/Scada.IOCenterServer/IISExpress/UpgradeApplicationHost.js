// 
// Updates an ApplicationHost.config file in the current user's
// profile directory from IIS 7.5 Express to IIS 10.0 Express.
// 

var strCommitPath = "MACHINE/WEBROOT/APPHOST";
var strFirstItem  = "##FIRST#ITEM##";
var strLastItem   = "##LAST#ITEM##";

// ------------------------------------------------------------
// Check for an existing ApplicationHost.config file.
// ------------------------------------------------------------

var objFSO = new ActiveXObject("scripting.filesystemobject");
var strUserConfig = GetUserDirectory() + "\\config\\ApplicationHost.config";
if (objFSO.FileExists(strUserConfig))
{
	// Backup existing an existing ApplicationHost.config file.
	try
	{
		var dtmDate = new Date();
		var strDate = dtmDate.getYear().toString() + 
			PadNumber(dtmDate.getMonth()+1) + 
			PadNumber(dtmDate.getDate()) + 
			PadNumber(dtmDate.getHours()) + 
			PadNumber(dtmDate.getMinutes()) + 
			PadNumber(dtmDate.getSeconds());
	 	objFSO.CopyFile(strUserConfig,strUserConfig + "." + strDate + ".bak",true);
	}
	catch(e)
	{
		ErrorMessage(e,"An error occurred trying to back up your ApplicationHost.config file");
	}
}
else
{
	// Exit if no applicationhost.config file exists. (This is not an error condition.)
	WScript.Echo("No ApplicationHost.config file exists in the current user's profile directory - exiting.");
	WScript.Quit(0);
}

// ------------------------------------------------------------
// Retrieve the necessary objects for the rest of the script.
// ------------------------------------------------------------

WScript.Echo("Migrating your ApplicationHost.config file...\n");
var objAdminManager      = GetAdminManager();
var objConfigManager     = objAdminManager.ConfigManager;
var objAppHostConfig     = objConfigManager.GetConfigFile(strCommitPath);
var objRootSectionGroup  = objAppHostConfig.RootSectionGroup;

// ------------------------------------------------------------
WScript.Echo("...adding new section groups...");
// ------------------------------------------------------------

var objSystemWebServer = FindSectionGroup(objRootSectionGroup,"system.webServer");
AddSection(objSystemWebServer,"applicationInitialization","Allow","MachineToApplication","");
AddSection(objSystemWebServer,"webSocket","Deny","","");
var objSecurity = FindSectionGroup(objSystemWebServer,"security");
AddSection(objSecurity,"dynamicIpSecurity","Deny","","");

// ------------------------------------------------------------
WScript.Echo("...adding new global modules...");
// ------------------------------------------------------------

var objGlobalModules = objAdminManager.GetAdminSection("system.webServer/globalModules", strCommitPath);
AddGlobalModule(objGlobalModules.Collection,"DynamicIpRestrictionModule","%IIS_BIN%\\diprestr.dll","","IpRestrictionModule");
AddGlobalModule(objGlobalModules.Collection,"ApplicationInitializationModule","%IIS_BIN%\\warmup.dll","","ConfigurationValidationModule");
AddGlobalModule(objGlobalModules.Collection,"WebSocketModule","%IIS_BIN%\\iiswsock.dll","","ApplicationInitializationModule");
AddGlobalModule(objGlobalModules.Collection,"ManagedEngine64","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\webengine.dll","integratedMode,runtimeVersionv2.0,bitness64","ManagedEngine");
AddGlobalModule(objGlobalModules.Collection,"ManagedEngineV4.0_64bit","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\webengine4.dll","integratedMode,runtimeVersionv4.0,bitness64","ManagedEngineV4.0_32bit");

// ------------------------------------------------------------
WScript.Echo("...adding new ISAPI filters...");
// ------------------------------------------------------------

var objIsapiFilters = objAdminManager.GetAdminSection("system.webServer/isapiFilters", strCommitPath);
AddIsapiFilter(objIsapiFilters.Collection,"ASP.Net_2.0.50727-64","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_filter.dll","","true","bitness64,runtimeVersionv2.0",strFirstItem);
AddIsapiFilter(objIsapiFilters.Collection,"ASP.Net_4.0_64bit","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_filter.dll","","true","bitness64,runtimeVersionv4.0","ASP.Net_4.0_32bit");

// ------------------------------------------------------------
WScript.Echo("...adding new ISAPI/CGI restrictions...");
// ------------------------------------------------------------

var objIsapiCgiRestrictions = objAdminManager.GetAdminSection("system.webServer/security/isapiCgiRestriction", strCommitPath);
AddIsapiCgiRestriction(objIsapiCgiRestrictions.Collection,"%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\webengine4.dll","true","ASP.NET_v4.0","ASP.NET_v4.0",strFirstItem);
AddIsapiCgiRestriction(objIsapiCgiRestrictions.Collection,"%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","true","ASP.NET v2.0.50727","ASP.NET v2.0.50727",strLastItem);
AddIsapiCgiRestriction(objIsapiCgiRestrictions.Collection,"%windir%\\Microsoft.NET\\Framework\\v2.0.50727\\aspnet_isapi.dll","true","ASP.NET v2.0.50727","ASP.NET v2.0.50727",strLastItem);

// ------------------------------------------------------------
WScript.Echo("...adding new MIME maps...");
// ------------------------------------------------------------

var objStaticContent = objAdminManager.GetAdminSection("system.webServer/staticContent", strCommitPath);
AddMimeMap(objStaticContent.Collection,".3g2","video/3gpp2");
AddMimeMap(objStaticContent.Collection,".3gp2","video/3gpp2");
AddMimeMap(objStaticContent.Collection,".3gp","video/3gpp");
AddMimeMap(objStaticContent.Collection,".3gpp","video/3gpp");
AddMimeMap(objStaticContent.Collection,".aac","audio/aac");
AddMimeMap(objStaticContent.Collection,".adt","audio/vnd.dlna.adts");
AddMimeMap(objStaticContent.Collection,".adts","audio/vnd.dlna.adts");
AddMimeMap(objStaticContent.Collection,".cab","application/vnd.ms-cab-compressed");
AddMimeMap(objStaticContent.Collection,".dvr-ms","video/x-ms-dvr");
AddMimeMap(objStaticContent.Collection,".eot","application/vnd.ms-fontobject");
AddMimeMap(objStaticContent.Collection,".js","application/javascript");
AddMimeMap(objStaticContent.Collection,".m2ts","video/vnd.dlna.mpeg-tts");
AddMimeMap(objStaticContent.Collection,".m4a","audio/mp4");
AddMimeMap(objStaticContent.Collection,".m4v","video/mp4");
AddMimeMap(objStaticContent.Collection,".mp4","video/mp4");
AddMimeMap(objStaticContent.Collection,".mp4v","video/mp4");
AddMimeMap(objStaticContent.Collection,".oga","audio/ogg");
AddMimeMap(objStaticContent.Collection,".ogg","video/ogg");
AddMimeMap(objStaticContent.Collection,".ogv","video/ogg");
AddMimeMap(objStaticContent.Collection,".ogx","application/ogg");
AddMimeMap(objStaticContent.Collection,".otf","font/otf");
AddMimeMap(objStaticContent.Collection,".spx","audio/ogg");
AddMimeMap(objStaticContent.Collection,".svg","image/svg+xml");
AddMimeMap(objStaticContent.Collection,".svgz","image/svg+xml");
AddMimeMap(objStaticContent.Collection,".ts","video/vnd.dlna.mpeg-tts");
AddMimeMap(objStaticContent.Collection,".tts","video/vnd.dlna.mpeg-tts");
AddMimeMap(objStaticContent.Collection,".webm","video/webm");
AddMimeMap(objStaticContent.Collection,".woff","font/x-woff");
AddMimeMap(objStaticContent.Collection,".wtv","video/x-ms-wtv");
AddMimeMap(objStaticContent.Collection,".xht","application/xhtml+xml");
AddMimeMap(objStaticContent.Collection,".xhtml","application/xhtml+xml");

// ------------------------------------------------------------
WScript.Echo("...adding new trace provider definitions...");
// ------------------------------------------------------------

var objTraceProviderDefinitions = objAdminManager.GetAdminSection("system.webServer/tracing/traceProviderDefinitions", strCommitPath);
AddTraceProviderDefinitions(objTraceProviderDefinitions.Collection,"WWW Server","WebSocket","16384");

// ------------------------------------------------------------
WScript.Echo("...updating trace areas...");
// ------------------------------------------------------------

var objTraceAreas = objAdminManager.GetAdminSection("system.webServer/tracing/traceFailedRequests", strCommitPath);
UpdateTraceAreas(objTraceAreas.Collection,"WWW Server","Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket","Verbose");

// ------------------------------------------------------------
WScript.Echo("...updating WebDAV global settings...");
// ------------------------------------------------------------

var objWebDavGlobalSettings = objAdminManager.GetAdminSection("system.webServer/webdav/globalSettings", strCommitPath);
UpdateWebDavGlobalSettings(objWebDavGlobalSettings.ChildElements.Item("propertyStores").Collection,"webdav_simple_prop","%IIS_BIN%\\webdav_simple_prop.dll","%IIS_BIN%\\webdav_simple_prop.dll");
UpdateWebDavGlobalSettings(objWebDavGlobalSettings.ChildElements.Item("lockStores").Collection,"webdav_simple_lock","%IIS_BIN%\\webdav_simple_lock.dll","%IIS_BIN%\\webdav_simple_lock.dll");

// ------------------------------------------------------------
WScript.Echo("...adding new modules...");
// ------------------------------------------------------------

var objModules = objAdminManager.GetAdminSection("system.webServer/modules", strCommitPath);
AddModule(objModules.Collection,"WebMatrixSupportModule","true","","","IISCertificateMappingAuthenticationModule");
AddModule(objModules.Collection,"DynamicIpRestrictionModule","true","","","IpRestrictionModule");
AddModule(objModules.Collection,"ApplicationInitializationModule","true","","","UrlMappingsModule");
AddModule(objModules.Collection,"WebSocketModule","true","","","ApplicationInitializationModule");
AddModule(objModules.Collection,"ConfigurationValidationModule","true","","","ServiceModel-4.0");

// ------------------------------------------------------------
WScript.Echo("...adding new handlers...");
// ------------------------------------------------------------

var objHandlers = objAdminManager.GetAdminSection("system.webServer/handlers", strCommitPath);
AddHandler(objHandlers.Collection,"vbhtml-ISAPI-4.0_64bit","*.vbhtml","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"vbhtm-ISAPI-4.0_64bit","*.vbhtm","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"cshtml-ISAPI-4.0_64bit","*.cshtml","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"cshtm-ISAPI-4.0_64bit","*.cshtm","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"aspq-ISAPI-4.0_64bit","*.aspq","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"xamlx-ISAPI-4.0_64bit","*.xamlx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","","",strFirstItem);
AddHandler(objHandlers.Collection,"xoml-ISAPI-4.0_64bit","*.xoml","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","","",strFirstItem);
AddHandler(objHandlers.Collection,"rules-ISAPI-4.0_64bit","*.rules","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","","",strFirstItem);
AddHandler(objHandlers.Collection,"svc-ISAPI-4.0_64bit","*.svc","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","","",strFirstItem);
AddHandler(objHandlers.Collection,"HttpRemotingHandlerFactory-soap-ISAPI-4.0_64bit","*.soap","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"HttpRemotingHandlerFactory-rem-ISAPI-4.0_64bit","*.rem","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"WebServiceHandlerFactory-ISAPI-4.0_64bit","*.asmx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"SimpleHandlerFactory-ISAPI-4.0_64bit","*.ashx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"PageHandlerFactory-ISAPI-4.0_64bit","*.aspx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"AXD-ISAPI-4.0_64bit","*.axd","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","",strFirstItem);
AddHandler(objHandlers.Collection,"svc-ISAPI-4.0_32bit","*.svc","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness32","","","HttpRemotingHandlerFactory-soap-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"rules-ISAPI-4.0_32bit","*.rules","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness32","","","svc-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"xoml-ISAPI-4.0_32bit","*.xoml","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness32","","","rules-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"xamlx-ISAPI-4.0_32bit","*.xamlx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness32","","","xoml-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"aspq-ISAPI-4.0_32bit","*.aspq","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness32","0","","xamlx-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"ScriptResourceIntegrated-4.0","*ScriptResource.axd","GET,HEAD","","System.Web.Handlers.ScriptResourceHandler, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35","","integratedMode,runtimeVersionv4.0","","","ScriptHandlerFactoryAppServices-Integrated-4.0");
AddHandler(objHandlers.Collection,"TraceHandler-Integrated","trace.axd","GET,HEAD,POST,DEBUG","","System.Web.Handlers.TraceHandler","","integratedMode,runtimeVersionv2.0","","","ISAPI-dll");
AddHandler(objHandlers.Collection,"WebAdminHandler-Integrated","WebAdmin.axd","GET,DEBUG","","System.Web.Handlers.WebAdminHandler","","integratedMode,runtimeVersionv2.0","","","TraceHandler-Integrated");
AddHandler(objHandlers.Collection,"AssemblyResourceLoader-Integrated","WebResource.axd","GET,DEBUG","","System.Web.Handlers.AssemblyResourceLoader","","integratedMode,runtimeVersionv2.0","","","WebAdminHandler-Integrated");
AddHandler(objHandlers.Collection,"PageHandlerFactory-Integrated","*.aspx","GET,HEAD,POST,DEBUG","","System.Web.UI.PageHandlerFactory","","integratedMode,runtimeVersionv2.0","","","AssemblyResourceLoader-Integrated");
AddHandler(objHandlers.Collection,"SimpleHandlerFactory-Integrated","*.ashx","GET,HEAD,POST,DEBUG","","System.Web.UI.SimpleHandlerFactory","","integratedMode,runtimeVersionv2.0","","","PageHandlerFactory-Integrated");
AddHandler(objHandlers.Collection,"svc-ISAPI-2.0-64","*.svc","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","","","HttpRemotingHandlerFactory-soap-ISAPI-2.0");
AddHandler(objHandlers.Collection,"AXD-ISAPI-2.0-64","*.axd","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","svc-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"PageHandlerFactory-ISAPI-2.0-64","*.aspx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","AXD-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"SimpleHandlerFactory-ISAPI-2.0-64","*.ashx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","PageHandlerFactory-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"WebServiceHandlerFactory-ISAPI-2.0-64","*.asmx","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","SimpleHandlerFactory-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"HttpRemotingHandlerFactory-rem-ISAPI-2.0-64","*.rem","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","WebServiceHandlerFactory-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"HttpRemotingHandlerFactory-soap-ISAPI-2.0-64","*.soap","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","0","","HttpRemotingHandlerFactory-rem-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"rules-64-ISAPI-2.0","*.rules","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","","","HttpRemotingHandlerFactory-soap-ISAPI-2.0-64");
AddHandler(objHandlers.Collection,"xoml-64-ISAPI-2.0","*.xoml","*","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v2.0.50727\\aspnet_isapi.dll","classicMode,runtimeVersionv2.0,bitness64","","","rules-64-ISAPI-2.0");
AddHandler(objHandlers.Collection,"SSINC-stm","*.stm","GET,HEAD,POST","ServerSideIncludeModule","","","","","File","CGI-exe");
AddHandler(objHandlers.Collection,"SSINC-shtm","*.shtm","GET,HEAD,POST","ServerSideIncludeModule","","","","","File","SSINC-stm");
AddHandler(objHandlers.Collection,"SSINC-shtml","*.shtml","GET,HEAD,POST","ServerSideIncludeModule","","","","","File","SSINC-shtm");
AddHandler(objHandlers.Collection,"ExtensionlessUrlHandler-ISAPI-4.0_64bit","*.","GET,HEAD,POST,DEBUG","IsapiModule","","%windir%\\Microsoft.NET\\Framework64\\v4.0.30319\\aspnet_isapi.dll","classicMode,runtimeVersionv4.0,bitness64","0","","ExtensionlessUrl-ISAPI-4.0_32bit");
AddHandler(objHandlers.Collection,"ExtensionlessUrl-Integrated-4.0","*.","GET,HEAD,POST,DEBUG","","System.Web.Handlers.TransferRequestHandler","","integratedMode,runtimeVersionv4.0","0","","ExtensionlessUrlHandler-ISAPI-4.0_64bit");

// ------------------------------------------------------------
// Commit changes and exit.
// ------------------------------------------------------------

try
{
	objAdminManager.CommitChanges();
}
catch(e)
{
	ErrorMessage(e,"An error occurred trying to commit the changes");
}

WScript.Echo("\nFinished!");
WScript.Quit(0);

// ================================================================================

function AddSection(tmpSectionGroup,tmpSectionName,tmpOverrideModeDefault,tmpAllowDefinition,tmpAllowLocation)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement2(tmpSectionGroup.sections,tmpSectionName);
		var tmpNewSection = null;
		if (tmpElementPosition == -1)
		{
			tmpNewSection = tmpSectionGroup.Sections.AddSection(tmpSectionName);
		}
		else
		{
			tmpNewSection = tmpSectionGroup.Sections.Item(tmpElementPosition);
		}
		// Add the required attributes.
		tmpNewSection.OverrideModeDefault = tmpOverrideModeDefault;
		tmpNewSection.AllowDefinition = tmpAllowDefinition;
		tmpNewSection.AllowLocation = tmpAllowLocation;
	}
	catch(e)
	{
		ErrorMessage(e,"An error occurred trying to add a section");
	}
}

// ================================================================================

function AddGlobalModule(tmpModuleGroup,tmpModuleName,tmpImage,tmpPreCondition,tmpPreviousModuleName)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpModuleGroup,"add",["name",tmpModuleName]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpModuleGroup.DeleteElement(tmpElementPosition);
		// Create a new element
		var tmpNewElement = tmpModuleGroup.CreateNewElement("add");
		// Add the required properties.
		tmpNewElement.Properties.Item("name").Value = tmpModuleName;
		tmpNewElement.Properties.Item("image").Value = tmpImage;
		// Add any optional properties.
		if (tmpPreCondition.length != 0) tmpNewElement.Properties.Item("preCondition").Value = tmpPreCondition;
		// Retrieve the previous index within the collection.
		tmpElementPosition = FindElement3(tmpModuleGroup,tmpPreviousModuleName);
		// Add the new element.
		tmpModuleGroup.AddElement(tmpNewElement, tmpElementPosition + ((tmpElementPosition>0) ? 1 : 0));
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add a global module");
	}
}

// ================================================================================

function AddIsapiFilter(tmpIsapiFilterCollection,tmpName,tmpPath,tmpEnabled,tmpEnableCache,tmpPreCondition,tmpPreviousFilterName)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpIsapiFilterCollection,"filter",["name",tmpName]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpIsapiFilterCollection.DeleteElement(tmpElementPosition);
		// Create a new element
		var tmpNewElement = tmpIsapiFilterCollection.CreateNewElement("filter");
		// Add the required properties.
		tmpNewElement.Properties.Item("name").Value = tmpName;
		tmpNewElement.Properties.Item("path").Value = tmpPath;
		// Add any optional properties.
		if (tmpEnabled.length != 0) tmpNewElement.Properties.Item("enabled").Value = tmpEnabled;
		if (tmpEnableCache.length != 0) tmpNewElement.Properties.Item("enableCache").Value = tmpEnableCache;
		if (tmpPreCondition.length != 0) tmpNewElement.Properties.Item("preCondition").Value = tmpPreCondition;
		// Retrieve the previous index within the collection.
		tmpElementPosition = FindElement3(tmpIsapiFilterCollection,tmpPreviousFilterName);
		// Add the new element.
		tmpIsapiFilterCollection.AddElement(tmpNewElement, tmpElementPosition + ((tmpElementPosition>0) ? 1 : 0));
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add an ISAPI filter");
	}
}

// ================================================================================

function AddIsapiCgiRestriction(tmpIsapiCgiRestrictionCollection,tmpPath,tmpAllowed,tmpGroupId,tmpDescription,tmpPrevious)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpIsapiCgiRestrictionCollection,"add",["path",tmpPath]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpIsapiCgiRestrictionCollection.DeleteElement(tmpElementPosition);
		// Create a new element
		var tmpNewElement = tmpIsapiCgiRestrictionCollection.CreateNewElement("add");
		// Add the required properties.
		tmpNewElement.Properties.Item("path").Value = tmpPath;
		tmpNewElement.Properties.Item("allowed").Value = tmpAllowed;
		// Add any optional properties.
		if (tmpGroupId.length != 0) tmpNewElement.Properties.Item("groupId").Value = tmpGroupId;
		if (tmpDescription.length != 0) tmpNewElement.Properties.Item("description").Value = tmpDescription;
		// Retrieve the previous index within the collection.
		tmpElementPosition = FindElement3(tmpIsapiCgiRestrictionCollection,tmpPrevious);
		// Add the new element.
		tmpIsapiCgiRestrictionCollection.AddElement(tmpNewElement, tmpElementPosition + ((tmpElementPosition>0) ? 1 : 0));
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add an ISAPI/CGI restriction");
	}
}

// ================================================================================

function AddMimeMap(tmpStaticContentCollection,tmpFileExtension,tmpMimeType)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpStaticContentCollection,"mimeMap",["fileExtension",tmpFileExtension]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpStaticContentCollection.DeleteElement(tmpElementPosition);
		// Create a new element
		var tmpNewElement = tmpStaticContentCollection.CreateNewElement("mimeMap");
		// Add the required properties.
		tmpNewElement.Properties.Item("fileExtension").Value = tmpFileExtension;
		tmpNewElement.Properties.Item("mimeType").Value = tmpMimeType;
		// Add the new element.
		tmpStaticContentCollection.AddElement(tmpNewElement, -1);
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add a MIME map");
	}
}

// ================================================================================

function AddTraceProviderDefinitions(tmpTraceProviderDefinitionCollection,tmpParent,tmpName,tmpValue)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition1 = FindElement1(tmpTraceProviderDefinitionCollection,"add",["name",tmpParent]);
		if (tmpElementPosition1 != -1)
		{
			var objWwwServerDefinitions = tmpTraceProviderDefinitionCollection.Item(tmpElementPosition1).ChildElements.Item(0).Collection;
			// Retrieve the index within the collection.
			var tmpElementPosition2 = FindElement1(objWwwServerDefinitions,"add",["name",tmpName]);
			// Delete the item if it already exists.
			if (tmpElementPosition2 != -1) objWwwServerDefinitions.DeleteElement(tmpElementPosition2);
			// Create a new element.
			var tmpNewElement = objWwwServerDefinitions.CreateNewElement("add");
			// Add the required properties.
			tmpNewElement.Properties.Item("name").Value = tmpName;
			tmpNewElement.Properties.Item("value").Value = tmpValue;
			// Add the new element.
			objWwwServerDefinitions.AddElement(tmpNewElement, -1);
		}
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add a trace provider definition");
	}
}

// ================================================================================

function UpdateTraceAreas(tmpTraceAreasCollection,tmpProvider,tmpAreas,tmpVerbosity)
{
	try
	{
		// Retrieve the index within the collection.
		var objTraceAreas = tmpTraceAreasCollection.Item(0).ChildElements.Item(0).Collection;
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(objTraceAreas,"add",["provider",tmpProvider]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) objTraceAreas.DeleteElement(tmpElementPosition);
		// Create a new element.
		var tmpNewElement = objTraceAreas.CreateNewElement("add");
		// Add the required properties.
		tmpNewElement.Properties.Item("provider").Value = tmpProvider;
		tmpNewElement.Properties.Item("areas").Value = tmpAreas;
		tmpNewElement.Properties.Item("verbosity").Value = tmpVerbosity;
		// Add the new element.
		objTraceAreas.AddElement(tmpNewElement, -1);
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to update the trace areas");
	}
}

// ================================================================================

function UpdateWebDavGlobalSettings(tmpWebDavStore,tmpName,tmpImage,tmpImage32)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpWebDavStore,"add",["name",tmpName]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpWebDavStore.DeleteElement(tmpElementPosition);
		// Create a new element.
		var tmpNewElement = tmpWebDavStore.CreateNewElement("add")
		// Add the required properties.
		tmpNewElement.Properties.Item("name").Value = tmpName;
		tmpNewElement.Properties.Item("image").Value = tmpImage;
		tmpNewElement.Properties.Item("image32").Value = tmpImage32;
		// Add the new element.
		tmpWebDavStore.AddElement(tmpNewElement, -1);
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to update the WebDAV settings");
	}
}

// ================================================================================

function AddModule(tmpModuleGroup,tmpModuleName,tmpLockItem,tmpType,tmpPreCondition,tmpPreviousModuleName)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpModuleGroup,"add",["name",tmpModuleName]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpModuleGroup.DeleteElement(tmpElementPosition);
		// Create a new element.
		var tmpNewElement = tmpModuleGroup.CreateNewElement("add");
		// Add the required properties.
		tmpNewElement.Properties.Item("name").Value = tmpModuleName;
		// Add any optional properties.
		if (tmpLockItem.length != 0) tmpNewElement.SetMetadata("lockItem", (tmpLockItem.toLowerCase() == "true") ? true : false );
		if (tmpType.length != 0) tmpNewElement.Properties.Item("type").Value = tmpType;
		if (tmpPreCondition.length != 0) tmpNewElement.Properties.Item("preCondition").Value = tmpPreCondition;
		// Retrieve the previous index within the collection.
		tmpElementPosition = FindElement3(tmpModuleGroup,tmpPreviousModuleName);
		// Add the new element.
		tmpModuleGroup.AddElement(tmpNewElement, tmpElementPosition + ((tmpElementPosition>0) ? 1 : 0));
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add a module");
	}
}

// ================================================================================

function AddHandler(tmpHandlerCollection,tmpName,tmpPath,tmpVerb,tmpModules,tmpType,tmpScriptProcessor,tmpPreCondition,tmpPesponseBufferLimit,tmpResourceType,tmpPrevious)
{
	try
	{
		// Retrieve the index within the collection.
		var tmpElementPosition = FindElement1(tmpHandlerCollection,"add",["name",tmpName]);
		// Delete the item if it already exists.
		if (tmpElementPosition != -1) tmpHandlerCollection.DeleteElement(tmpElementPosition);
		// Create a new element.
		var tmpNewElement = tmpHandlerCollection.CreateNewElement("add");
		// Add the required properties.
		tmpNewElement.Properties.Item("name").Value = tmpName;
		tmpNewElement.Properties.Item("verb").Value = tmpVerb;
		tmpNewElement.Properties.Item("path").Value = tmpPath;
		// Add any optional properties.
		if (tmpType.length != 0) tmpNewElement.Properties.Item("type").Value = tmpType;
		if (tmpModules.length != 0) tmpNewElement.Properties.Item("modules").Value = tmpModules;
		if (tmpScriptProcessor.length != 0) tmpNewElement.Properties.Item("scriptProcessor").Value = tmpScriptProcessor;
		if (tmpPreCondition.length != 0) tmpNewElement.Properties.Item("preCondition").Value = tmpPreCondition;
		if (tmpPesponseBufferLimit.length != 0) tmpNewElement.Properties.Item("responseBufferLimit").Value = tmpPesponseBufferLimit;
		if (tmpResourceType.length != 0) tmpNewElement.Properties.Item("resourceType").Value = tmpResourceType;
		// Retrieve the previous index within the collection.
		tmpElementPosition = FindElement3(tmpHandlerCollection,tmpPrevious);
		// Add the new element.
		tmpHandlerCollection.AddElement(tmpNewElement, tmpElementPosition + ((tmpElementPosition>0) ? 1 : 0));
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to add a handler");
	}
}

// ================================================================================

function PadNumber(tmpNumber)
{
	return (tmpNumber < 10) ? ("0" + tmpNumber.toString()) : tmpNumber.toString();
}

// ================================================================================

function ErrorMessage(tmpError,tmpMessage)
{
	WScript.Echo("\n" + tmpMessage + ":\n" + tmpError.description);
	WScript.Quit(tmpError.number);
}

// ================================================================================

function GetAdminManager()
{
	try
	{
		var tmpVersionManager = WScript.CreateObject("Microsoft.IIS.VersionManager");
		var tmpVersionObject = tmpVersionManager.GetVersionObject("10.0", 1);
		var tmpAdminManager = tmpVersionObject.CreateObjectFromProgId("Microsoft.ApplicationHost.WritableAdminManager");
		return tmpAdminManager;
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to obtain the Admin Manager");
	}
}

// ================================================================================

function GetUserDirectory()
{
	try
	{
		var tmpVersionManager = WScript.CreateObject("Microsoft.IIS.VersionManager");
		var tmpVersionObject = tmpVersionManager.GetVersionObject("10.0", 1);
		var tmpUserData = tmpVersionObject.GetPropertyValue("userInstanceHelper")
		var tmpUserDirectory = tmpUserData.IISDirectory;
		if (tmpUserDirectory.length > 0) return tmpUserDirectory;
		throw("The User Directory cannot be determined.");
	}
	catch(e)
	{
		ErrorMessage(e,"The following error occurred trying to obtain the User Directory");
	}
}

// ================================================================================

function FindSectionGroup(tmpParentSectionGroup,tmpName)
{
	try
	{
		// Retrieve the index within the sectionGroup.
		var tmpElementPosition = FindElement2(tmpParentSectionGroup,tmpName);
		// Fail completely if we can't retrive the index.
		if (tmpElementPosition == -1) throw("Cannot retrieve index for '" & tmpName & "'.");
		return tmpParentSectionGroup.Item(tmpElementPosition);
	}
	catch(e)
	{
		ErrorMessage(e,"An error occurred trying to add a section group");
	}
}

// ================================================================================

function FindElement1(tmpCollection, tmpElementTagName, tmpValuesArray)
{
   for (var tmpCount1 = 0; tmpCount1 < tmpCollection.Count; ++tmpCount1)
   {
      var tmpElement = tmpCollection.Item(tmpCount1);
      if (tmpElement.Name == tmpElementTagName)
      {
         var tmpMatches = true;
         for (var tmpCount2 = 0; tmpCount2 < tmpValuesArray.length; tmpCount2 += 2)
         {
            var tmpProperty = tmpElement.GetPropertyByName(tmpValuesArray[tmpCount2]);
            var tmpValue = tmpProperty.Value;
            if (tmpValue != null) tmpValue = tmpValue.toString();
            if (tmpValue != tmpValuesArray[tmpCount2 + 1])
            {
               tmpMatches = false;
               break;
            }
         }
         if (tmpMatches) return tmpCount1;
      }
   }
   return -1;
}

// ================================================================================

function FindElement2(tmpCollection,tmpName)
{
	for (var tmpCount = 0; tmpCount < tmpCollection.Count; ++tmpCount)
	{
		var tmpElement = tmpCollection.Item(tmpCount);
		if (tmpElement.Name == tmpName)
		{
			return tmpCount;
		}
	}
   return -1;
}

// ================================================================================

function FindElement3(tmpCollection,tmpName)
{
	if ((tmpName.length ==0) || (tmpName.toLowerCase() == strLastItem.toLowerCase())) return -1;
	if ((tmpName.length ==0) || (tmpName.toLowerCase() == strFirstItem.toLowerCase())) return 0;	
	return FindElement1(tmpCollection,"add",["name",tmpName]);
}

// SIG // Begin signature block
// SIG // MIIjkAYJKoZIhvcNAQcCoIIjgTCCI30CAQExDzANBglg
// SIG // hkgBZQMEAgEFADB3BgorBgEEAYI3AgEEoGkwZzAyBgor
// SIG // BgEEAYI3AgEeMCQCAQEEEBDgyQbOONQRoqMAEEvTUJAC
// SIG // AQACAQACAQACAQACAQAwMTANBglghkgBZQMEAgEFAAQg
// SIG // qkLf6hUIUQIUdckZ0Q9IsyYTk/q7ULZ8ux3vr03b6zOg
// SIG // gg2BMIIF/zCCA+egAwIBAgITMwAAAd9r8C6Sp0q00AAA
// SIG // AAAB3zANBgkqhkiG9w0BAQsFADB+MQswCQYDVQQGEwJV
// SIG // UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
// SIG // UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
// SIG // cmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBT
// SIG // aWduaW5nIFBDQSAyMDExMB4XDTIwMTIxNTIxMzE0NVoX
// SIG // DTIxMTIwMjIxMzE0NVowdDELMAkGA1UEBhMCVVMxEzAR
// SIG // BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
// SIG // bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
// SIG // bjEeMBwGA1UEAxMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
// SIG // MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA
// SIG // trsZWRAAo6nx5LhcqAsHy9uaHyPQ2VireMBI9yQUOPBj
// SIG // 7dVLA7/N+AnKFFDzJ7P+grT6GkOE4cv5GzjoP8yQJ6yX
// SIG // ojEKkXti7HW/zUiNoF11/ZWndf8j1Azl6OBjcD416tSW
// SIG // Yvh2VfdW1K+mY83j49YPm3qbKnfxwtV0nI9H092gMS0c
// SIG // pCUsxMRAZlPXksrjsFLqvgq4rnULVhjHSVOudL/yps3z
// SIG // OOmOpaPzAp56b898xC+zzHVHcKo/52IRht1FSC8V+7QH
// SIG // TG8+yzfuljiKU9QONa8GqDlZ7/vFGveB8IY2ZrtUu98n
// SIG // le0WWTcaIRHoCYvWGLLF2u1GVFJAggPipwIDAQABo4IB
// SIG // fjCCAXowHwYDVR0lBBgwFgYKKwYBBAGCN0wIAQYIKwYB
// SIG // BQUHAwMwHQYDVR0OBBYEFDj2zC/CHZDRrQnzJlT7byOl
// SIG // WfPjMFAGA1UdEQRJMEekRTBDMSkwJwYDVQQLEyBNaWNy
// SIG // b3NvZnQgT3BlcmF0aW9ucyBQdWVydG8gUmljbzEWMBQG
// SIG // A1UEBRMNMjMwMDEyKzQ2MzAwOTAfBgNVHSMEGDAWgBRI
// SIG // bmTlUAXTgqoXNzcitW2oynUClTBUBgNVHR8ETTBLMEmg
// SIG // R6BFhkNodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtp
// SIG // b3BzL2NybC9NaWNDb2RTaWdQQ0EyMDExXzIwMTEtMDct
// SIG // MDguY3JsMGEGCCsGAQUFBwEBBFUwUzBRBggrBgEFBQcw
// SIG // AoZFaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9w
// SIG // cy9jZXJ0cy9NaWNDb2RTaWdQQ0EyMDExXzIwMTEtMDct
// SIG // MDguY3J0MAwGA1UdEwEB/wQCMAAwDQYJKoZIhvcNAQEL
// SIG // BQADggIBAJ56h7Q8mFBWlQJLwCtHqqup4aC/eUmULt0Z
// SIG // 6We7XUPPUEd/vuwPuIa6+1eMcZpAeQTm0tGCvjACxNNm
// SIG // rY8FoD3aWEOvFnSxq6CWR5G2XYBERvu7RExZd2iheCqa
// SIG // EmhjrJGV6Uz5wmjKNj16ADFTBqbEBELMIpmatyEN50UH
// SIG // wZSdD6DDHDf/j5LPGUy9QaD2LCaaJLenKpefaugsqWWC
// SIG // MIMifPdh6bbcmxyoNWbUC1JUl3HETJboD4BHDWSWoDxI
// SIG // D2J4uG9dbJ40QIH9HckNMyPWi16k8VlFOaQiBYj09G9s
// SIG // LMc0agrchqqZBjPD/RmszvHmqJlSLQmAXCUgcgcf6UtH
// SIG // EmMAQRwGcSTg1KsUl6Ehg75k36lCV57Z1pC+KJKJNRYg
// SIG // g2eI6clzkLp2+noCF75IEO429rjtujsNJvEcJXg74TjK
// SIG // 5x7LqYjj26Myq6EmuqWhbVUofPWm1EqKEfEHWXInppqB
// SIG // YXFpBMBYOLKc72DT+JyLNfd9utVsk2kTGaHHhrp+xgk9
// SIG // kZeud7lI/hfoPeHOtwIc0quJIXS+B5RSD9nj79vbJn1J
// SIG // x7RqusmBQy509Kv2Pg4t48JaBfBFpJB0bUrl5RVG05sK
// SIG // /5Qw4G6WYioS0uwgUw499iNC+Yud9vrh3M8PNqGQ5mJm
// SIG // JiFEjG2ToEuuYe/e64+SSejpHhFCaAFcMIIHejCCBWKg
// SIG // AwIBAgIKYQ6Q0gAAAAAAAzANBgkqhkiG9w0BAQsFADCB
// SIG // iDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0
// SIG // b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
// SIG // Y3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWlj
// SIG // cm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5
// SIG // IDIwMTEwHhcNMTEwNzA4MjA1OTA5WhcNMjYwNzA4MjEw
// SIG // OTA5WjB+MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
// SIG // aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
// SIG // ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSgwJgYDVQQD
// SIG // Ex9NaWNyb3NvZnQgQ29kZSBTaWduaW5nIFBDQSAyMDEx
// SIG // MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA
// SIG // q/D6chAcLq3YbqqCEE00uvK2WCGfQhsqa+laUKq4Bjga
// SIG // BEm6f8MMHt03a8YS2AvwOMKZBrDIOdUBFDFC04kNeWSH
// SIG // fpRgJGyvnkmc6Whe0t+bU7IKLMOv2akrrnoJr9eWWcpg
// SIG // GgXpZnboMlImEi/nqwhQz7NEt13YxC4Ddato88tt8zpc
// SIG // oRb0RrrgOGSsbmQ1eKagYw8t00CT+OPeBw3VXHmlSSnn
// SIG // Db6gE3e+lD3v++MrWhAfTVYoonpy4BI6t0le2O3tQ5GD
// SIG // 2Xuye4Yb2T6xjF3oiU+EGvKhL1nkkDstrjNYxbc+/jLT
// SIG // swM9sbKvkjh+0p2ALPVOVpEhNSXDOW5kf1O6nA+tGSOE
// SIG // y/S6A4aN91/w0FK/jJSHvMAhdCVfGCi2zCcoOCWYOUo2
// SIG // z3yxkq4cI6epZuxhH2rhKEmdX4jiJV3TIUs+UsS1Vz8k
// SIG // A/DRelsv1SPjcF0PUUZ3s/gA4bysAoJf28AVs70b1FVL
// SIG // 5zmhD+kjSbwYuER8ReTBw3J64HLnJN+/RpnF78IcV9uD
// SIG // jexNSTCnq47f7Fufr/zdsGbiwZeBe+3W7UvnSSmnEyim
// SIG // p31ngOaKYnhfsi+E11ecXL93KCjx7W3DKI8sj0A3T8Hh
// SIG // hUSJxAlMxdSlQy90lfdu+HggWCwTXWCVmj5PM4TasIgX
// SIG // 3p5O9JawvEagbJjS4NaIjAsCAwEAAaOCAe0wggHpMBAG
// SIG // CSsGAQQBgjcVAQQDAgEAMB0GA1UdDgQWBBRIbmTlUAXT
// SIG // gqoXNzcitW2oynUClTAZBgkrBgEEAYI3FAIEDB4KAFMA
// SIG // dQBiAEMAQTALBgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUw
// SIG // AwEB/zAfBgNVHSMEGDAWgBRyLToCMZBDuRQFTuHqp8cx
// SIG // 0SOJNDBaBgNVHR8EUzBRME+gTaBLhklodHRwOi8vY3Js
// SIG // Lm1pY3Jvc29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9N
// SIG // aWNSb29DZXJBdXQyMDExXzIwMTFfMDNfMjIuY3JsMF4G
// SIG // CCsGAQUFBwEBBFIwUDBOBggrBgEFBQcwAoZCaHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNS
// SIG // b29DZXJBdXQyMDExXzIwMTFfMDNfMjIuY3J0MIGfBgNV
// SIG // HSAEgZcwgZQwgZEGCSsGAQQBgjcuAzCBgzA/BggrBgEF
// SIG // BQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3Br
// SIG // aW9wcy9kb2NzL3ByaW1hcnljcHMuaHRtMEAGCCsGAQUF
// SIG // BwICMDQeMiAdAEwAZQBnAGEAbABfAHAAbwBsAGkAYwB5
// SIG // AF8AcwB0AGEAdABlAG0AZQBuAHQALiAdMA0GCSqGSIb3
// SIG // DQEBCwUAA4ICAQBn8oalmOBUeRou09h0ZyKbC5YR4WOS
// SIG // mUKWfdJ5DJDBZV8uLD74w3LRbYP+vj/oCso7v0epo/Np
// SIG // 22O/IjWll11lhJB9i0ZQVdgMknzSGksc8zxCi1LQsP1r
// SIG // 4z4HLimb5j0bpdS1HXeUOeLpZMlEPXh6I/MTfaaQdION
// SIG // 9MsmAkYqwooQu6SpBQyb7Wj6aC6VoCo/KmtYSWMfCWlu
// SIG // WpiW5IP0wI/zRive/DvQvTXvbiWu5a8n7dDd8w6vmSiX
// SIG // mE0OPQvyCInWH8MyGOLwxS3OW560STkKxgrCxq2u5bLZ
// SIG // 2xWIUUVYODJxJxp/sfQn+N4sOiBpmLJZiWhub6e3dMNA
// SIG // BQamASooPoI/E01mC8CzTfXhj38cbxV9Rad25UAqZaPD
// SIG // XVJihsMdYzaXht/a8/jyFqGaJ+HNpZfQ7l1jQeNbB5yH
// SIG // PgZ3BtEGsXUfFL5hYbXw3MYbBL7fQccOKO7eZS/sl/ah
// SIG // XJbYANahRr1Z85elCUtIEJmAH9AAKcWxm6U/RXceNcbS
// SIG // oqKfenoi+kiVH6v7RyOA9Z74v2u3S5fi63V4GuzqN5l5
// SIG // GEv/1rMjaHXmr/r8i+sLgOppO6/8MO0ETI7f33VtY5E9
// SIG // 0Z1WTk+/gFcioXgRMiF670EKsT/7qMykXcGhiJtXcVZO
// SIG // SEXAQsmbdlsKgEhr/Xmfwb1tbWrJUnMTDXpQzTGCFWcw
// SIG // ghVjAgEBMIGVMH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQI
// SIG // EwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4w
// SIG // HAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAm
// SIG // BgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENB
// SIG // IDIwMTECEzMAAAHfa/AukqdKtNAAAAAAAd8wDQYJYIZI
// SIG // AWUDBAIBBQCgga4wGQYJKoZIhvcNAQkDMQwGCisGAQQB
// SIG // gjcCAQQwHAYKKwYBBAGCNwIBCzEOMAwGCisGAQQBgjcC
// SIG // ARUwLwYJKoZIhvcNAQkEMSIEIL3c5hcMzDGgIsZFzIq1
// SIG // RYrcAG7g1GxWjD8jmhmqz8buMEIGCisGAQQBgjcCAQwx
// SIG // NDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBhodHRw
// SIG // Oi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEB
// SIG // BQAEggEAXZ6INNhO3QFhfzN2T0HDexqyLd2OWQ35lxFw
// SIG // fnKmYtnfihVP5nWKN24tu4kBZ9tN8pZ6D/HZJlwALAQN
// SIG // 0mfZ0y90w3++498euUr7KOt/jvg+9B/bGfpuBKNjXvVT
// SIG // SogCtHKo/bcSWeBVo4nHmieusW1oYDPO9LcoWlSulZtP
// SIG // iITOvY0Aml1ybLxR0bGOrE379quakVtovHWw4r8owuFx
// SIG // 4JcTYxsx2t+YBAaIJWcmMAZ2LrLBEB3AvnLY4SGLNQ3m
// SIG // Pf9+vfnrPj3hcslah4sAN9HNcTkkkqq5hnjTTXgJ5qec
// SIG // UxUSTbQAm2zTxuyA78KPJc/NFZB+cdIM4o5cBqTOxaGC
// SIG // EvEwghLtBgorBgEEAYI3AwMBMYIS3TCCEtkGCSqGSIb3
// SIG // DQEHAqCCEsowghLGAgEDMQ8wDQYJYIZIAWUDBAIBBQAw
// SIG // ggFVBgsqhkiG9w0BCRABBKCCAUQEggFAMIIBPAIBAQYK
// SIG // KwYBBAGEWQoDATAxMA0GCWCGSAFlAwQCAQUABCAH1nQI
// SIG // Sr71t8wkhcSJ4z9LtxEUCEtAu3BJYiJv/Cp1GgIGYUTT
// SIG // N2shGBMyMDIxMDkyNzE4NTA1Ny41MjdaMASAAgH0oIHU
// SIG // pIHRMIHOMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
// SIG // aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
// SIG // ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSkwJwYDVQQL
// SIG // EyBNaWNyb3NvZnQgT3BlcmF0aW9ucyBQdWVydG8gUmlj
// SIG // bzEmMCQGA1UECxMdVGhhbGVzIFRTUyBFU046NDYyRi1F
// SIG // MzE5LTNGMjAxJTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1l
// SIG // LVN0YW1wIFNlcnZpY2Wggg5EMIIE9TCCA92gAwIBAgIT
// SIG // MwAAAVhwWiL3vpbmAwAAAAABWDANBgkqhkiG9w0BAQsF
// SIG // ADB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGlu
// SIG // Z3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMV
// SIG // TWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1N
// SIG // aWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDAeFw0y
// SIG // MTAxMTQxOTAyMTRaFw0yMjA0MTExOTAyMTRaMIHOMQsw
// SIG // CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQ
// SIG // MA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
// SIG // b2Z0IENvcnBvcmF0aW9uMSkwJwYDVQQLEyBNaWNyb3Nv
// SIG // ZnQgT3BlcmF0aW9ucyBQdWVydG8gUmljbzEmMCQGA1UE
// SIG // CxMdVGhhbGVzIFRTUyBFU046NDYyRi1FMzE5LTNGMjAx
// SIG // JTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1lLVN0YW1wIFNl
// SIG // cnZpY2UwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEK
// SIG // AoIBAQChHwuXYPWrsNCgBRsL9e8jBRvEn6oFFBQvA88G
// SIG // vJq6bNHsoUUNjb/Su/7M/31RNaP9X2aeKuEhorXLIzxr
// SIG // Tp41seOVSBUyDUKXaDoZrD3Zxct4AV6TBrU316i551BO
// SIG // PlZigtrwITmdOlOr7eQnNHCaKhCbczlkcBGs/AaF9pwl
// SIG // 9UQV5B9z4gLu7Vib91fM4UUjyxZnoifgiMGstOAFIJq8
// SIG // FxEB7yR4G+j4iwsYBNlQAQgzU+QlconjWqXGYisdekGw
// SIG // 5XuyjsJIzBCCpHMUft9nQzLcwraSFA4KysZo8fhpveIx
// SIG // 4nqITh1LoZd7t4ZQGH79kgP/Ok9VDQIgUIN1rvcbAgMB
// SIG // AAGjggEbMIIBFzAdBgNVHQ4EFgQUS3DZG32dHBgf7ud+
// SIG // oHuTJ9Oi+VgwHwYDVR0jBBgwFoAU1WM6XIoxkPNDe3xG
// SIG // G8UzaFqFbVUwVgYDVR0fBE8wTTBLoEmgR4ZFaHR0cDov
// SIG // L2NybC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVj
// SIG // dHMvTWljVGltU3RhUENBXzIwMTAtMDctMDEuY3JsMFoG
// SIG // CCsGAQUFBwEBBE4wTDBKBggrBgEFBQcwAoY+aHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNU
// SIG // aW1TdGFQQ0FfMjAxMC0wNy0wMS5jcnQwDAYDVR0TAQH/
// SIG // BAIwADATBgNVHSUEDDAKBggrBgEFBQcDCDANBgkqhkiG
// SIG // 9w0BAQsFAAOCAQEAOd8oA1qL0K4fH7pYjV1tAlAU83wO
// SIG // EpeIfiDxIeZTXa4Qxcuk+DAPY7qdc85RZKWK1HNLE30A
// SIG // gDpwI5rpz4J5mkuW0n9lR/DIN+FNqoDyyJzAJBmgbPwc
// SIG // 2myeuWCntT+SCmTe1o9m0XwitNxEvJEu4OmEB+u4sTAk
// SIG // Aiw63lgyiWLDbNHITaSTgM8iXhn8kVHvk1FGxcI7Av9f
// SIG // CpmDg1YKUUmGcdFu46xqpSVRHobsKUiLBjmAgTJyQzXS
// SIG // pz/tdwoOvHFbQjV+pCXb1BR9GYrjzJQWA+xqwj6gEZUp
// SIG // /r8X3zIr7tgzCSS5HssMUnw+drA1fjQX+SJ4rihXBPct
// SIG // JvZtozCCBnEwggRZoAMCAQICCmEJgSoAAAAAAAIwDQYJ
// SIG // KoZIhvcNAQELBQAwgYgxCzAJBgNVBAYTAlVTMRMwEQYD
// SIG // VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25k
// SIG // MR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24x
// SIG // MjAwBgNVBAMTKU1pY3Jvc29mdCBSb290IENlcnRpZmlj
// SIG // YXRlIEF1dGhvcml0eSAyMDEwMB4XDTEwMDcwMTIxMzY1
// SIG // NVoXDTI1MDcwMTIxNDY1NVowfDELMAkGA1UEBhMCVVMx
// SIG // EzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1Jl
// SIG // ZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3Jh
// SIG // dGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3Rh
// SIG // bXAgUENBIDIwMTAwggEiMA0GCSqGSIb3DQEBAQUAA4IB
// SIG // DwAwggEKAoIBAQCpHQ28dxGKOiDs/BOX9fp/aZRrdFQQ
// SIG // 1aUKAIKF++18aEssX8XD5WHCdrc+Zitb8BVTJwQxH0Eb
// SIG // GpUdzgkTjnxhMFmxMEQP8WCIhFRDDNdNuDgIs0Ldk6zW
// SIG // czBXJoKjRQ3Q6vVHgc2/JGAyWGBG8lhHhjKEHnRhZ5Ff
// SIG // gVSxz5NMksHEpl3RYRNuKMYa+YaAu99h/EbBJx0kZxJy
// SIG // GiGKr0tkiVBisV39dx898Fd1rL2KQk1AUdEPnAY+Z3/1
// SIG // ZsADlkR+79BL/W7lmsqxqPJ6Kgox8NpOBpG2iAg16Hgc
// SIG // sOmZzTznL0S6p/TcZL2kAcEgCZN4zfy8wMlEXV4WnAEF
// SIG // TyJNAgMBAAGjggHmMIIB4jAQBgkrBgEEAYI3FQEEAwIB
// SIG // ADAdBgNVHQ4EFgQU1WM6XIoxkPNDe3xGG8UzaFqFbVUw
// SIG // GQYJKwYBBAGCNxQCBAweCgBTAHUAYgBDAEEwCwYDVR0P
// SIG // BAQDAgGGMA8GA1UdEwEB/wQFMAMBAf8wHwYDVR0jBBgw
// SIG // FoAU1fZWy4/oolxiaNE9lJBb186aGMQwVgYDVR0fBE8w
// SIG // TTBLoEmgR4ZFaHR0cDovL2NybC5taWNyb3NvZnQuY29t
// SIG // L3BraS9jcmwvcHJvZHVjdHMvTWljUm9vQ2VyQXV0XzIw
// SIG // MTAtMDYtMjMuY3JsMFoGCCsGAQUFBwEBBE4wTDBKBggr
// SIG // BgEFBQcwAoY+aHR0cDovL3d3dy5taWNyb3NvZnQuY29t
// SIG // L3BraS9jZXJ0cy9NaWNSb29DZXJBdXRfMjAxMC0wNi0y
// SIG // My5jcnQwgaAGA1UdIAEB/wSBlTCBkjCBjwYJKwYBBAGC
// SIG // Ny4DMIGBMD0GCCsGAQUFBwIBFjFodHRwOi8vd3d3Lm1p
// SIG // Y3Jvc29mdC5jb20vUEtJL2RvY3MvQ1BTL2RlZmF1bHQu
// SIG // aHRtMEAGCCsGAQUFBwICMDQeMiAdAEwAZQBnAGEAbABf
// SIG // AFAAbwBsAGkAYwB5AF8AUwB0AGEAdABlAG0AZQBuAHQA
// SIG // LiAdMA0GCSqGSIb3DQEBCwUAA4ICAQAH5ohRDeLG4Jg/
// SIG // gXEDPZ2joSFvs+umzPUxvs8F4qn++ldtGTCzwsVmyWrf
// SIG // 9efweL3HqJ4l4/m87WtUVwgrUYJEEvu5U4zM9GASinbM
// SIG // QEBBm9xcF/9c+V4XNZgkVkt070IQyK+/f8Z/8jd9Wj8c
// SIG // 8pl5SpFSAK84Dxf1L3mBZdmptWvkx872ynoAb0swRCQi
// SIG // PM/tA6WWj1kpvLb9BOFwnzJKJ/1Vry/+tuWOM7tiX5rb
// SIG // V0Dp8c6ZZpCM/2pif93FSguRJuI57BlKcWOdeyFtw5yj
// SIG // ojz6f32WapB4pm3S4Zz5Hfw42JT0xqUKloakvZ4argRC
// SIG // g7i1gJsiOCC1JeVk7Pf0v35jWSUPei45V3aicaoGig+J
// SIG // FrphpxHLmtgOR5qAxdDNp9DvfYPw4TtxCd9ddJgiCGHa
// SIG // sFAeb73x4QDf5zEHpJM692VHeOj4qEir995yfmFrb3ep
// SIG // gcunCaw5u+zGy9iCtHLNHfS4hQEegPsbiSpUObJb2sgN
// SIG // VZl6h3M7COaYLeqN4DMuEin1wC9UJyH3yKxO2ii4sanb
// SIG // lrKnQqLJzxlBTeCG+SqaoxFmMNO7dDJL32N79ZmKLxvH
// SIG // Ia9Zta7cRDyXUHHXodLFVeNp3lfB0d4wwP3M5k37Db9d
// SIG // T+mdHhk4L7zPWAUu7w2gUDXa7wknHNWzfjUeCLraNtvT
// SIG // X4/edIhJEqGCAtIwggI7AgEBMIH8oYHUpIHRMIHOMQsw
// SIG // CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQ
// SIG // MA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
// SIG // b2Z0IENvcnBvcmF0aW9uMSkwJwYDVQQLEyBNaWNyb3Nv
// SIG // ZnQgT3BlcmF0aW9ucyBQdWVydG8gUmljbzEmMCQGA1UE
// SIG // CxMdVGhhbGVzIFRTUyBFU046NDYyRi1FMzE5LTNGMjAx
// SIG // JTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1lLVN0YW1wIFNl
// SIG // cnZpY2WiIwoBATAHBgUrDgMCGgMVAKnJK3Ma59ELIabq
// SIG // M46fpfg0nzS/oIGDMIGApH4wfDELMAkGA1UEBhMCVVMx
// SIG // EzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1Jl
// SIG // ZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3Jh
// SIG // dGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3Rh
// SIG // bXAgUENBIDIwMTAwDQYJKoZIhvcNAQEFBQACBQDk/IBz
// SIG // MCIYDzIwMjEwOTI3MjE0MDAzWhgPMjAyMTA5MjgyMTQw
// SIG // MDNaMHcwPQYKKwYBBAGEWQoEATEvMC0wCgIFAOT8gHMC
// SIG // AQAwCgIBAAICH+kCAf8wBwIBAAICEeswCgIFAOT90fMC
// SIG // AQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYKKwYBBAGEWQoD
// SIG // AqAKMAgCAQACAwehIKEKMAgCAQACAwGGoDANBgkqhkiG
// SIG // 9w0BAQUFAAOBgQCoF8AVAnTza+7TikYV4t2lbUXUqMyQ
// SIG // k7m+dqj0tfrRHQ4yq6Qolqcg19zFgOtzoxiG92kM02Fn
// SIG // ouLpGSvxiIWJs+IRV/Xu/YwiUklebe8bLTNDO26mTlP+
// SIG // B5IoUsCS0IjwRtd2HC1UnPbKefSfbqBJ1rRDZUlPN44j
// SIG // PTeGjzdCfjGCAw0wggMJAgEBMIGTMHwxCzAJBgNVBAYT
// SIG // AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
// SIG // EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
// SIG // cG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1l
// SIG // LVN0YW1wIFBDQSAyMDEwAhMzAAABWHBaIve+luYDAAAA
// SIG // AAFYMA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkqhkiG9w0B
// SIG // CQMxDQYLKoZIhvcNAQkQAQQwLwYJKoZIhvcNAQkEMSIE
// SIG // ILLetS5jc/OctFNF6jFjbnCK5Nd6H46nthlabzXSZ24r
// SIG // MIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCBvQQg8koz
// SIG // jWyGNZdsyk+G2uLAiOFpAQurCH0fbklTVcdw0wcwgZgw
// SIG // gYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
// SIG // aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
// SIG // ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQD
// SIG // Ex1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMAIT
// SIG // MwAAAVhwWiL3vpbmAwAAAAABWDAiBCAHjaKCUVVgcXGn
// SIG // yhP0SPpV3HyIxp6hRSFo9srRgtM1rzANBgkqhkiG9w0B
// SIG // AQsFAASCAQAkgHzmQccw76BEpES4zGyYcKiNW6b2Gp0T
// SIG // FGWycBIef6OXkccOeY6lbkMQ4gVCIFxUvSCo+0cifaeg
// SIG // rmEFNj1JPDkQdxWMtwM5Y39gwKpEb+AkA8ZPxjxROdlr
// SIG // 1uahShZA9hrt5jLBZxGnHe8et98obXhtAjbFVR0XwKMY
// SIG // VW52Hy+R5wkx92Ly4fXO4PrKP++/pFekNs3/m8ygqteW
// SIG // 3ATOIzMknX4FV5lzoaeM2i7vlKxxicAWmahIErmX/zyh
// SIG // JB1XWZ5j8HsZseqfXC8PnVlHYZnmwq0EeXdETW8f4JOW
// SIG // cFAzPN8aLtSvmmNTvKTKMVspFzD3x1ZjXZL6g1yvNfia
// SIG // End signature block
