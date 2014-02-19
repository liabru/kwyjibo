# Kwyjibo #

[brm.io/kwyjibo](http://brm.io/kwyjibo)

_Kwy·ji·bo, n._

	“A big, dumb, balding North American ape with no chin and a short temper.”

— Bart Simpson

Kwyjibo is also an experimental, automated referee for Scrabble games. It was built to explore real-time OCR, computer vision and machine learning as the subject of my dissertation at the [University of Sheffield](http://www.shef.ac.uk/) in 2011.

## About ##

You can read a little bit about how it works in this [explanation](http://brm.io/real-time-ocr/). 
Much of the image processing is done using the awesome [Aforge.NET](http://www.aforgenet.com/) computer vision library. 
There's also a [video](http://www.youtube.com/watch?v=c3ywTfeTqOE) of it working on shefcompsci's channel.

Since this was just a research project while I was an undergraduate, it's obviously not anything ground breaking. 
That also means the code is a bit ugly and doesn't always follow best-practices due to time constraints. 
I don't usually like releasing too much rough code, but I don't mind since a few interested Scrabble players requested it.

It may not work with your own board and camera, but feel free to give it a go, if it works give me a [shout](http://brm.io/kwyjibo#comments)! 

My implementation of the Scrabble game rules is a little shaky (it's a worse referee than [Homer](http://www.youtube.com/watch?v=MtM6tJaPzFI)).
I'm not really a player and I didn't have much time for that bit, but the computer vision part works nicely. 

Read more about this thing here in my [post](http://brm.io/real-time-ocr/).

_Note_: this project was for academic research only and is not associated with Scrabble ®

## Try it out ##

If you just want to try it out, you'll need a web cam, download the executable from the [Kwyjibo page](http://brm.io/kwyjibo) and run it. Follow the instructions shown to calibrate it to your board and camera.

You can also poke around with the source code on [github](https://github.com/liabru/kwyjibo), you'll need Visual Studio installed to build it.

## License ##

Kwyjibo is licensed under [The MIT License (MIT)](http://opensource.org/licenses/MIT) 
<br/>Copyright (c) 2014 Liam Brummitt

[Aforge.NET](http://www.aforgenet.com/) libraries licensed under [LGPL v3](http://www.gnu.org/copyleft/lesser.html)

This license is also supplied with the release and source code. 
As stated in the license, absolutely no warranty is provided.