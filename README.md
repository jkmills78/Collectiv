# Collectiv

Collectiv is a free open-source multiplatform software for collection tracking. If you like my work, please consider [buying me a coffee](https://buymeacoffee.com/jkmills78).

##  First things first...

I am just an old simple programmer and a collector of (hopefully) older things, and have provided you with a free open-source application for your own personal use.  As such, I am not responsible for your use or misuse of this software, its source code, or anything that it produces.  There are components that could be used in a public facing environment.  I would strongly recommend against that.  That is an incredibly bad idea for many reasons.  This is so important that it is worth repeating and with bold print, **do not try to use this software or it's generated content in a public environment as it currently is**.  Do not make me pull out the CAPS LOCK.

I built this application with the intention of it being for personal local use on a computer-like device or for limited hosting within a trusted local network, i.e. a homelab.  I have not added anything remotely even close to adequate security.

## What is it?

Collectiv is an application for cataloging and tracking whatever it is that you collect.  It does this by allowing you to define attributes for each collection.  As you add items, these attributes are able to be populated with a value.  For instance, if I am tracking a video game collection, it may have such attributes as ```Title```, ```Platform```, ```Publisher```, and ```Condition```, which I have defined for this particular collection under its settings.  When I add a new item to this collection, in this instance a game, I could then provide values under that new item for each of these attributes.   These attributes can be anything you wish, and you can have as many as you would like.  You can also associate files, i.e. documents, photos, et cetera, with any collection or item.  I use the word associate specifically here, as Collectiv does not store any of these files itself, rather it simply saves the path to the files.  This was an intentional design decision to have it work this way, as it allows the filesystem to be structured in a different hierarchy from the collection details themselves.  Collectiv is built primarily for Windows desktop users in mind.  It should also work on Macs, but I don't own one to test with.  It can also be used on Android or iOS mobile devices, but these devices will probably work best with the included Web API in a locally hosted envrionment.  More on this in the [detailed user guide](/Resources/Documentation/DetailedUserGuide/DetailedUserGuide.md).

## Why did you build it?

Glad you asked!  I built Collectiv because I have a lot of vintage computers and software, but I couldn't find a way to catalog my collectibles to my complete contentment.  I have many folders and files which are associated with my collection and that I want to keep track of.  Sometimes I know I have a thing, but can't remember where I put it.  Sometimes I want a thing, but don't know if I own it already.  That's when I had a moderately intelligent idea! I'm an ok programmer, so I can just build something to keep track of it all!  There must be something wrong with me since I obviously don't get enough of this programming nonsense during my day job.

## What can it do now?

* Create and manage multiple collections of items
* Create and manage collections of associated files for the collection
* Doing almost anything is automatically saved in Collectiv's internal database (there are various confirmation steps to ensure you don't do anything unintentional)
* Packages of files can be added under both the collection itself as a whole and under individual items
* Create your own custom attributes in the collection which are propagated down to all of the items in that collection
* Populate the value of these attributes in the items, and hopefully soon use them to search with
* Use your existing files (your files are not stored in Collectiv, only the collection data)
* Operate in local mode for desktop systems or in hosted mode (in tandem with the included Web API) for mobile devices

### Use on a Mobile Device

Using Collectiv on a mobile device is much different from using it on a desktop device.  The biggest difference is that most people probably don't store all of the files associates with their collections on their mobile device.  On top of that, mobile devices have varying ways in which file storage is handled.  Because of this, I also created a file server that can be accessed remotely to handle this piece (see ```First things first...``` where I discuss the security implications of this).  The web API allows mobile devices to access the collection files remotely.  Please note that the internal database which stores the collection data is aways local to the application, so only the files are served.  I have provided a prebuilt docker image for use in this mode of operation, which you can find in [my docker hub](https://hub.docker.com/r/jkmills78/collectivfileserver).  Some NASes provide the capability of hosting docker images, so you may already have everything you need to set this up.  Once the file server is running on some remote host, make sure you go into the Collectiv Settings screen and change it to hosted mode, also providing the host address and port here.  When configuring the docker file, you will need to specify some additional settings. Port ```32770``` will need to be mapped to the docker container's port ```8080``` and port ```32771``` will need to be mapped to the docker container's port ```8081```.  You will also need to configure a volume using whatever host path you choose and it should be mapped to the ```/UserData``` volume inside of the docker image where it is already defined.

## How can I contribute?

The interface design is adequate at best.  I am a programmer, not an artist, so all I can say is that I have done my best on that side.  If you want to help on that, I would be thrilled.  I also have never been much of a Mac person, so there are almost certainly Mac-specific issues that need to be corrected.  Better instructions for use would be nice to have as well.  Dare I dream, maybe even something "artsy" with animations?  There could also be some features that are as yet unfinished.  You will find those under the Issues section here in the repository.  By the time anyone even reads this, they may already be done, so check often.

## Who can I thank?

The following projects/tools have been used in the creation of Collectiv.  Therefore, as a small token of appreciation, I am linking back to their projects so that their owners are explicitly credited for their work.

### Open-Source Contributors

There are just too many worthy contributions to list here.  That's what I'll be saying in 5 years.  I hope.  In all seriousness though, you are at the top of the list of those to thank.  I don't care if you know so little about programming that you can't even spell C#.  If all you even do is provide feedback, then you have been an invaluable contributor.

### Inno Setup

https://jrsoftware.org/isinfo.php

Inno Setup is a tool created by Jordan Russell and is just an absolute pleasure to work with.  It's stupid how easy this utility created an installer for my unpackaged Maui code.  Finally something that "just works".

### IconFont2Code

https://andreinitescu.github.io/IconFont2Code/

This tool was used to extract the unicode values from the font packs so that glyphs could be used instead of images.  I'm not really sure what I would have done for the glyphs if I hadn't found this.

### ByteSize

https://github.com/omar/ByteSize

This tool allows file sizes to be printed out as human-readable strings.  Super cool!

### MimeDetective

https://github.com/MediatedCommunications/Mime-Detective

This tool allows the discovery of mime types from the binary data, rather than just simply relying on the file extension.  I am almost certainly not using this in the optimal way.

### SQLite

https://www.sqlite.org/index.html

This is a lightweight portable database, which I use for the internal storage of Collectiv.  Neat!

### Microsoft

https://dotnet.microsoft.com/en-us/apps/maui

.NET 8 Maui is the Microsoft framework upon which this is built, using Microsoft Visual Studio 2022, for my own personal Microsoft Windows 11 desktop.  Yeah, that's a lot of Microsoft.  I literally could not have built this without them.

### Me

Last of all, me.  You can thank me.  You can thank me by [buying me a coffee](https://buymeacoffee.com/jkmills78) if you really like my work enough.  Maybe you think I'm a garbage programmer in need of more skills, and that's ok too.  If that is the case, you can [buy me a coffee](https://buymeacoffee.com/jkmills78) because I am going to have some long nights ahead trying to catch up to all of you who put the pro in programmer.  I greatly appreciate any generosity contributed towards my humble work.  Thank you all.
