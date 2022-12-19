## TO-DO List to get the SDKs working

### FIREBASE ###

	1) Create a firebase project and download the google-services.json into Assets folder
	2) Make sure the package name (com.gamina.xx) is the same on the dashboards and in unity
	3) Add the FirebaseManager script on an object in the scene

### ADMOST ###

	1) Create an app on the admost dashboard and create zones
	2) Make sure the package name (com.gamina.xx) is the same on the dashboards and in unity
	3) Fill the zone ids in the AdsManager script (AdIds enum below the script)
	4) Add the AdsManager script on an object in the scene
	5) Go to Player Settings > Publishing Settings and check the boxes for the following:
		
		- Custom Main Manifest 
		- Custom Main Gradle Template 
		- Custom Base Gradle Template
		- Custom Gradle Properties Template

	6) Uncheck all the Minify boxes
	7) Go to Assets > External Dependency Manager > Android Resolver > Settings and check Jetifier box
	8) Go to Custom Gradle Properties Template file and add the following lines if missing:

		- android.useAndroidX=true
		- android.enableJetifier=true

	9) Go to Assets > External Dependency Manager > Android Resolver and Resolve
	10) You may need to add google() repository to the repositories { } tag in mainTemplate.gradle file if missing
	11) Open mainTemplate.gradle file and add multiDexEnabled true and multidex library as below.
		
		android {
    			defaultConfig {
      	  			....
      	  			multiDexEnabled true
      	  			....
			}
		}
	
	12) Open AndroidManifest.xml file and add the android:name tag as below:
		
		<manifest xmlns:android="http://schemas.android.com/apk/res/android"
		    package="com.amr.definition">
			<application
		        ...
            		//If you use AndroidX packages
			android:name="androidx.multidex.MultiDexApplication" >
		        ...
			</application>
		</manifest>

### Facebook ###

	1) From the top bar go to Facebook > Edit Settings and fill in the App ID and Client Token
	2) Make sure the package name (com.gamina.xx) is the same on the dashboards and in unity
	3) Add the MetaManager script on an object in the scene

### Event Logging ###

	1) Add the EventManager script on an object in the scene
	2) To log an event, call "EventsManager.Instance.log(Event, parameters);"
