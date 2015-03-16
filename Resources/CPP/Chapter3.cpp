#include <iostream>
#include <string>
#include <conio.h>
#include <math.h>
#include <ctime>

using namespace std;

int Factorial(int n);
char ToUpperCase(char input);
char ToLowerCase(char input);
float dist3(float ux, float uy, float uz,
			float vx, float vy, float vz);
void PrintDistance(float a[], float b[], float distance);
void calcDistance(float a[], float b[]);
float MyArcTangent(float y, float x);
float MyArcTangent2(float y, float x);
bool isValidBet(int bet, int playerChips);
int Random(int low, int high);
int getPlayerBet(int playerChips);
int getPayout(int slot1, int slot2, int slot3, int playerBet);
int BinSearch(int data[], int numElements, int searchKey);
void printarray (int arg[], int length);
void BubbleSort(int data[], int n);

int main() 
{
	//EXERCISE 3.7.1 Factorial
	/*bool quit = false;
	while(!quit)
	{
	int baseN = 0;

	cout << "Enter a positive integer to compute the factorial of: ";
	cin >> baseN;
	if(baseN > 0) {
	int result = Factorial(baseN);
	cout << baseN << "! = " << result << endl;
	}
	else 
	{
	quit = true;
	}
	}*/

	//EXERCISE 3.7.2 ToUpper; ToLower
	/*cout << "Lower case to Upper:" << endl;
	for(int i=97;i<97+26;i++) 
	{
	cout<<ToUpperCase(char(i));
	}
	cout << endl;
	cout << endl;
	cout << "Upper case to Lower:" << endl;
	for(int i=65;i<65+26;i++) 
	{
	cout<<ToLowerCase(char(i));
	}
	cout << endl;
	cout << endl;*/

	//EXERCISE 3.7.3 3D Distance
	/*float a[3] = { 1.0f,2.0f,3.0f };
	float b[3] = {0.0f,0.0f,0.0f};
	float distance = dist3(a[0], a[1], a[2], b[0], b[1], b[2]);
	PrintDistance(a, b, distance);
	b[0] = 1.0f;
	b[1] = 2.0f;
	b[2] = 3.0f;
	distance = dist3(a[0], a[1], a[2], b[0], b[1], b[2]);
	PrintDistance(a, b, distance);
	b[0] = 7.0f;
	b[1] = -4.0f;
	b[2] = 5.0f;
	distance = dist3(a[0], a[1], a[2], b[0], b[1], b[2]);
	PrintDistance(a, b, distance);*/

	//EXCERCISE 3.7.4 Arc Tangent 2
	/*cout << "MyArcTangent( 4, 2) = " << MyArcTangent2(4, 2) << endl;
	cout << "MyArcTangent( 5, -1) = " << MyArcTangent2(5, -1) << endl;
	cout << "MyArcTangent( -4, -6) = " << MyArcTangent2(-4, -6) << endl;
	cout << "MyArcTangent( -6, 4) = " << MyArcTangent2(-6, 4) << endl;*/

	//EXERCISE 3.7.5 Calculator Program
	/*bool userQuits = false;
	double x,y;
	while(!userQuits) {
	int command;
	cout << "*** CALCULATOR ***" << endl;
	cout << "1) cos(x), 2) sin(x), 3) tan(x), 4) atam2(y, x), 5) sqrt(x), 6) x^y ";
	cout << "7) ln(x), 8) e^x, 9) |x|, 10) floor(x), 11) ceil(x), 12) Exit" << endl;
	cin >> command;
	cout << endl;
	switch(command) {
	case(1):
	cout << "X = ";
	cin >> x;
	cout << "cos(" << x << ") = " << cos(x) << endl;
	break;
	case(2):
	cout << "X = ";
	cin >> x;
	cout << "sin(" << x << ") = " << sin(x) << endl;
	break;
	case(3):
	cout << "X = ";
	cin >> x;
	cout << "tan(" << x << ") = " << tan(x) << endl;
	break;
	case(4):
	cout << "Y = ";
	cin >> y;
	cout << "X = ";
	cin >> x;
	cout << "atan2(" << y << "," << x << ") = " << atan2f(y,x) << endl;
	break;
	case(5):
	cout << "X = ";
	cin >> x;
	cout << "sqrt(" << x << ") = " << sqrt(x) << endl;
	break;
	case(6):
	cout << "X = ";
	cin >> x;
	cout << "Y = ";
	cin >> y;
	cout << x << "^" << y << " = " << pow(x, y) << endl;
	break;
	case(7):
	cout << "X = ";
	cin >> x;
	cout << "ln(" << x << ") = " << log(x) << endl;
	break;
	case(8):
	cout << "X = ";
	cin >> x;
	cout << "e^" << x << " = " << exp(x) << endl;
	break;
	case(9):
	cout << "X = ";
	cin >> x;
	cout << "|" << x << "| = " << abs(x) << endl;
	break;
	case(10):
	cout << "X = ";
	cin >> x;
	cout << "floor(" << x << ") = " << floor(x) << endl;
	break;
	case(11):
	cout << "X = ";
	cin >> x;
	cout << "ceil(" << x << ") = " << ceil(x) << endl;
	break;
	default: 
	cout << "Exiting...";
	userQuits = true;
	break;
	}
	cout << endl;
	}*/

	//EXERCISE 3.7.6 Slot Machine
	/*srand(time(0));
	int playerChips = 1000;
	bool gameExit = false;
	int slotArray[3] = { 0,0,0 }; 
	int playerInput, playerBet, payout;

	while(!gameExit) {
	cout << "Player's chips: " << playerChips << endl;
	cout << "1) Play slot.  2) Exit.";
	cin >> playerInput;
	if(playerInput == 1) {
	int playerBet = getPlayerBet(playerChips);
	slotArray[0] = Random(2,7);
	slotArray[1] = Random(2,7);
	slotArray[2] = Random(2,7);
	cout << slotArray[0] << " " << slotArray[1] << " " << slotArray[2] << endl;
	int payout = getPayout(slotArray[0], slotArray[1], slotArray[2], playerBet);
	if(payout > 0) {
	playerChips += payout;
	}
	else {
	playerChips -= playerBet;
	}
	if(playerChips == 0) {
	cout << "exiting...";
	gameExit = true;
	}
	}
	else if(playerInput == 2) {
	cout << "exiting...";
	gameExit = true;
	}
	}*/

	//EXERCISE 3.7.7 Binary Search
	//int dataSize = 23;
	//int searchKey = 0;
	//string searchValue = "";
	//bool quit = false;
	//int data[23] = { 1,4,5,6,9,14,21,23,28,31,35,42,46,50,53,57,62,63,65,74,79,89,95 };
	//cout << "{";
	//for(int i=0;i<dataSize;i++) {
	//	cout << " " << data[i];
	//	if(i<dataSize -1) {
	//		cout << ",";
	//	}
	//}
	//cout << " }" << endl;
	//while(!quit) {
	//	cout << "Enter search key (or 'x' to Exit):";
	//	cin >> searchValue;
	//	//printarray(data, dataSize);
	//	if(searchValue == "x" )
	//	{
	//		cout << "exiting..." << endl;
	//		quit = true;
	//	}
	//	else {
	//		searchKey = atoi(searchValue.c_str()); 
	//		int position = BinSearch(data, dataSize, searchKey);
	//		if(position == -1) {
	//			cout << "search key not found." << endl;
	//		}
	//		else {
	//			cout << searchKey << " is in position " << position << "." << endl;
	//		}
	//	}
	//}

	//EXCERCISE 3.7.8 Bubble Sort
	int data[10];
	cout << "Enter ten unsorted integers..." << endl;
	for(int i=0;i<10;i++) 
	{
		cout << "[" << i << "] = ";
		cin >> data[i];
	}
	cout << "unsorted List = ";
	printarray(data, 10);
	cout << endl;
	cout << "Sorting..." << endl << endl;
	BubbleSort(data, 10);
	cout << "Sorted List = ";
	printarray(data, 10);
	cout << endl;
}

void BubbleSort(int data[], int n) 
{
	int subArrayEnd = n -1;
	while (subArrayEnd > 0) {
		int nextEnd = 0;
		for(int i=0; i<subArrayEnd;i++) 
		{
			int a = data[i];
			int b = data[i+1];
			if(a > b) 
			{
				data[i] = b;
				data[i+1] = a;
				nextEnd = i;
			}
		}
		subArrayEnd = nextEnd;
	}
}
void printarray (int arg[], int length) {
  for (int n=0; n<length; n++)
    cout << arg[n] << " ";
  cout << "\n";
}
int BinSearch(int data[], int numElements, int searchKey) {
	int retVal = -1;
	int midIndex = 0;
	int startIndex = 0;
	int endIndex = numElements - 1;
	bool searchKeyFound = false;
	while(endIndex >= startIndex && !searchKeyFound) {
		midIndex = (endIndex + startIndex) / 2;
		if(data[midIndex] == searchKey) {
			retVal = midIndex;
			searchKeyFound = true;
		}
		else if(searchKey < data[midIndex]) {
			endIndex = midIndex - 1; 
		}
		else if(searchKey > data[midIndex]) {
			startIndex = midIndex + 1;
		}
	}
	return retVal;

}
int getPayout(int slot1, int slot2, int slot3, int playerBet) 
{
	int payout = 0;
	if(slot1 == 7 && slot2 == 7 && slot3 == 7) 
	{
		payout = playerBet * 10;
	}
	else if(slot1 == slot2 && slot2 == slot3) {
		payout = playerBet * 5;
	}
	else if(slot1 == slot2 || slot2 == slot3 || slot1 == slot3) {
		payout = playerBet * 3;
	}
	return payout;
}
int getPlayerBet(int playerChips) {
	int playerBet = 0;
	cout << "Enter your bet: ";
	cin >> playerBet;
	if(playerBet <= playerChips && playerBet > 0) {
		return playerBet;
	}
	else {
		cout << "You did not enter a valid bet.";
		getPlayerBet(playerChips);
	}
}
int Random(int low, int high) {
	int returnVal = 0;
	returnVal = low + rand() % (high - low);
	return returnVal;
}
float MyArcTangent(float y, float x) {
	float theta;
	if(x > 0) {
		theta = atanf(y/x)*180/3.14;
	}
	else {
		theta = atanf(y/x)*180/3.14 + 180;
	}
	return theta;
}
float MyArcTangent2(float y, float x) {
	float thetaf = atan2f(y, x)*180/3.14;
	return thetaf;
}
float dist3(float ux, float uy, float uz, float vx, float vy, float vz) 
{
	float dist1 = (vx - ux) * (vx-ux) + (vy - uy) * (vy - uy) + (vz - uz) * (vz - uz);
	float distance = sqrt(dist1); 
	return distance;
}
void PrintDistance(float a[3], float b[3], float distance) 
{
	cout << "Distances between (" << a[0] << ", " << a[1] << ", " << a[2] << ") and ";
	cout << "(" << b[0] << ", " << b[1] << ", " << b[2] << " = " << distance << endl;
}
int Factorial(int n) 
{
	int result = 0;
	int factorialCounter = 0;
	result = n;
	factorialCounter = n - 1;
	while(factorialCounter > 1) {
		result *= factorialCounter;
		factorialCounter--;
	}
	return result;
}
char ToUpperCase(char input)
{
	return input -= 32;
}
char ToLowerCase(char input)
{
	return input += 32;
}