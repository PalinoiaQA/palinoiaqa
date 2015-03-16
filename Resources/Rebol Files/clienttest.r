REBOL [title: "Client Test"]
view layout [
	a: area 500x500
	button "Test" [
		msg: a/text
		message: rejoin ["Message: " msg] 
		print message
		write tcp://simcps.no-ip.biz:4321 { print "Hello" } 
	]
]