# Collectiv

Collectiv is a free open-source multiplatform software for collection tracking. If you like my work, please consider [buying me a coffee](https://buymeacoffee.com/jkmills78).

##  First things first...

I am just an old simple programmer and a collector of (hopefully) older things, and have provided you with a free open-source application for your own personal use.  As such, I am not responsible for your use or misuse of this software, its source code, or anything that it produces.  There are components that could be used in a public facing environment.  I would strongly recommend against that.  That is an incredibly bad idea for many reasons.  This is so important that it is worth repeating and with bold print, **do not try to use this software or it's generated content in a public environment as it currently is**.  Do not make me pull out the CAPS LOCK.

I built this application with the intention of it being for personal local use on a computer-like device or for limited hosting within a trusted local network, i.e. a homelab.  I have not added anything remotely even close to adequate security.

## What is it?

Collectiv is an application for cataloging and tracking whatever it is that you collect.  It does this by allowing you to define attributes for each collection.  As you add items, these attributes are able to be populated with a value.  For instance, if I am tracking a video game collection, it may have such attributes as ```Title```, ```Platform```, ```Publisher```, and ```Condition```, which I have defined for this particular collection under its settings.  When I add a new item to this collection, in this instance a game, I could then provide values under that new item for each of these attributes.   These attributes can be anything you wish, and you can have as many as you would like.  You can also associate files, i.e. documents, photos, et cetera, with any collection or item.  I use the word associate specifically here, as Collectiv does not store any of these files itself, rather it simply saves the path to the files.  This was an intentional design decision to have it work this way, as it allows the filesystem to be structured in a different hierarchy from the collection details themselves.  Collectiv is built primarily for Windows desktop users in mind.  It should also work on Macs, but I don't own one to test with.  It can also be used on Android or iOS mobile devices, but these devices will probably work best with the included Web API in a locally hosted envrionment.  More on this in the detailed guide below.

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

## Detailed Guide

This detailed guide will be written using a contrived scenario so that it is easier to understand.  I personally enjoy collecting both video games and antique hand tools.  I'm sure anyone would agree that these are very different collections, and as such will have unique attributes associated with each.  Due to the flexibility of Collectiv, there are different ways you can create your collection, so you may choose to adopt a different structure from what I am exemplifying.

Step 1.)  First I will create two new entries under collections.  One I will name ```Floppy Disks```, and the other ```Hand Tools```.  So far so good.

Step 2.)  Since these are different types of things, they will have different important attributes, so I will create those independently for each collection.  I will create three attributes for each collection as an example.  I click on the ```Floppy Disks``` collection header and I see the ```Collection Details``` page.  This is the page that displays your items and files.  I actually need to go a bit further in, so I click on the button for ```Collection Settings```.  This is the page that allows you to create attributes.  For ```Floppy Disks``` I create the attributes ```Platform```, ```Publisher```, and ```Condition```.  Once added these attributes will be added to any existing items in my collection.  I have none yet, so let's continue.

Step 3.)  I back out into ```Collection Settings```.  This is the page where I can add items and file packages to my collection.  Let's add an item first.  I will name this item ```Loom```.

Step 4.)  Once I have added the item, I click on its header to enter the ```Item Details``` page.  This page is where I can add file packages for items and any attribute values that I created previously under the collection.  Let's do the attributes first.  I see three attributes listed: ```Platform```, ```Publisher```, and ```Condition```, none of which have any values yet.  I edit each of these to add their proper values.  For ```Platform``` I will put ```PC```.  For publisher, as both of us already know, this will be the venerable ```LucasArts```.  Finally, for ```Condition``` I will say ```Good```.

Now I am ready to associate some files with this item.  I could add these associations directly under the ```Collection Details``` page for display in its file gallery at the top with the corresponding items listed out underneath, but I prefer to keep the files inside of the item itself.  I will be adding two ```File Packages``` to the ```Item Details```.  Remember that Collectiv does not store your files, only the locations of those files.  Even in ```Hosted``` mode, the files are simply stored at whatever location you have provided.

Step 5.a)  I click the button to add a new ```File Package```, and I am directed to the ```File Package Details``` page.  I give it a title of ```Box Contents```.  I then give it a suitably long-winded description, befitting it's name.  I will put ```Stuff that came inside the box```.  I will only use one disk in this example since it will be the same for the rest of the items.  I already have a picture I took of the disk and a backup image (which I will refer to as the .img file from now on since I keep throwing about similar words like picture and image) of the disk itself. I click the button to add a new file and select the picture, then I click the button to add another file and select the .img file.  I am done with this ```File Package```, so now I will click on ```Save```.

This is the one place where you can find a save button rather than a confirmation button.  That's because there are too many things on this page to confirm each, so I just put a save button to save all the things all at once.

Step 5.b)  Now I add the second and final ```File Package```.  I repeat the steps above, only for the title I put ```Box Art```, for the description I put ```Complete box picture set```, I add all associated pictures, and I click on ```Save``` again.

Now in my ```Item Details``` page, I will have the file gallery in the top portion that I can scroll horizontally between to see my two packages: ```Box Contents``` and ```Box Art```.  If I have set one of the picture as primary, then it will be displayed in the gallery as a cover image.  I will see my item attributes set at the bottom of the page for ```Platform PC```, ```Publisher LucasArts```, and ```Condition Good```

Our first collection has been set up.  Now let's look at the ```Hand Tools```.  This will be much shorter since I have already outlined the steps.  One big difference that I want to point out though, is that while most of the same things can be done in this collection, one step is a bit different.  Gold star for the person who thought "I'm pretty sure hand tools don't have publishers or platforms."  This is where the flexibility begins to shine.  Instead of adding attributes for ```Platform``` or ```Publisher``` to the ```Collection Details```, I will add ```Tool Type``` and ```Manufacturer```, and instead of putting the values ```PC``` and ```LucasArts```, I will put ```Stanley``` and ```No. 5 Jack Plane```.

Our second collection has now been set up.

## How can I contribute?

The interface design is adequate at best.  I am a programmer, not an artist, so all I can say is that I have done my best on that side.  If you want to help on that, I would be thrilled.  I also have never been much of a Apple person, so there are almost certainly Apple-specific issues that need to be corrected.  Better instructions for use would be nice to have as well.  Dare I dream, maybe even something "artsy" with animations?  There could also be some features that are as yet unfinished.  You will find those under the Issues section here in the repository.  By the time anyone even reads this, they may already be done, so check often.

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