//Wiktor Nocon
//Program uklada pentonimo o polu 60 z 12 roznych klockow. Nr ulozenia oznacza ktory z kolei ulozyny klocek zostanie
//wyswietlony na konsoli.

#include <iostream>
#include <ctime>
#include <Windows.h>
#include <vector>
#include <list>

using namespace std;

struct Point {
	int x;
	int y;
};
class Brick {
private:
	Point pos;//pozycja klocka jesli jest na boardzie
	Point * currentArrOfPoint;// ksztal klocka jest opisany przez 5 punktow na tablicy 5X5
	Point **allArrOfPoint;
	int * arrIndexOfMinXYPoint;//ktory z punktow jest przy danych zorientawaniu jest najwyzej i jak najbardziej na lewo 
	const int lenArrOfPoint = 5;//klocki skladaja z 5 mniejszych klockow 1X1
	const int lenArrOfOrientation;//na ile sposob mozna orientowac klocek
	const char c;//litera przypisana danemu klockowi ktora przypomina ksztaltem klocek
	Point* CreateCopyArrPoint(Point * arrPoint) {
		Point * result = new Point[5];
		for (int i = 0; i < lenArrOfPoint; i++) {
			result[i].x = arrPoint[i].x;
			result[i].y = arrPoint[i].y;
		}
		return result;
	}
	Point* CreateArrPointRotated(Point * arrPoint) {
		Point *result = CreateCopyArrPoint(arrPoint);
		int minX=5, minY=5;
		int tmp;
		for (int i = 0; i < 5; i++) {
			tmp = result[i].y;
			result[i].y = 4 - result[i].x;
			result[i].x = tmp;
			if (result[i].x < minX)minX = result[i].x;
			if (result[i].y < minY) minY = result[i].y;
		}
		for (int i = 0; i < 5; i++) {
			result[i].x=result[i].x - minX;
			result[i].y = result[i].y - minY;
		}	
		return result;
	}
	Point* CreateArrPointFliped(Point * arrPoint) {
		Point *result = CreateCopyArrPoint(arrPoint);
		int maxX = 0;
		for (int i = 0; i < 5; i++) {
			if (result[i].x > maxX)maxX = result[i].x;
		}
		for (int i = 0; i < 5; i++) {
			result[i].x = maxX - result[i].x;
		}
		return result;
	}
	int GetMinYXPoint(Point * arrPoint) {
		int result = 0;
		int minY = arrPoint[0].y;
		for (int i = 1; i < lenArrOfPoint; i++) {
			if (arrPoint[i].y < minY) {
				minY = arrPoint[i].y;
				result = i;
			}
			else if (minY == arrPoint[i].y && arrPoint[i].x < arrPoint[result].x) result = i;
			
		}
		return result;
	}
	int GetCurrentMinYXPoint() { return arrIndexOfMinXYPoint[orientation]; }

public:
	int orientation;
	Brick(Point * arr ,int nOrientation,  bool * arrOfOrientation, char _c ) : lenArrOfOrientation(nOrientation) , c(_c) {
		orientation = 0;
		pos.x = 0;
		pos.y = 0;
		currentArrOfPoint = arr;
		allArrOfPoint = new Point*[lenArrOfOrientation];
		allArrOfPoint[0] = new Point[5];
		allArrOfPoint[0] = arr;
		for (int i = 1; i < lenArrOfOrientation; i++) {
			allArrOfPoint[i] = new Point[5];
			if (arrOfOrientation[i] == 0) allArrOfPoint[i] = CreateArrPointRotated(allArrOfPoint[i-1]);
			else allArrOfPoint[i] = CreateArrPointFliped(allArrOfPoint[i - 1]);
		}
		arrIndexOfMinXYPoint = new int[lenArrOfOrientation];
		for (int i = 0; i < lenArrOfOrientation; i++) {
			arrIndexOfMinXYPoint[i] = GetMinYXPoint(allArrOfPoint[i]);
		}
		delete[] arrOfOrientation;
	}
	void SetPos(int x , int y) {pos.x=x; pos.y=y;}
	bool IsMatch(char ** arr2d,int a , int b ,Point pToCover) {
		Point pTemp;
		int i = 0;
		int indexOfHighestPoint = GetCurrentMinYXPoint();
		Point p;
		p.x = pToCover.x - currentArrOfPoint[indexOfHighestPoint].x;
		p.y = pToCover.y - currentArrOfPoint[indexOfHighestPoint].y;
		//sprawdza czy pozostale klocki pasuja
		while (i <lenArrOfPoint) {
			pTemp.x = p.x + currentArrOfPoint[i].x;
			pTemp.y = p.y + currentArrOfPoint[i].y;
			if (pTemp.x < 0 || pTemp.y <0 || pTemp.x >= a || pTemp.y >= b || arr2d[pTemp.x][pTemp.y] != 0) {
				return false;
			}
			i++;
		}
		pos = p;
		return true;
	}
	void NextPosition() {//zmienia na nastepne orientowanie klocka
		orientation++;
		if (orientation == lenArrOfOrientation)orientation = 0;
		currentArrOfPoint = allArrOfPoint[orientation];
	}
	void TakeFromArr2d(char ** arr2d) {
		for (int i = 0; i < lenArrOfPoint; i++) {
			arr2d[pos.x + currentArrOfPoint[i].x][pos.y + currentArrOfPoint[i].y] = 0;
		}
	}
	void PutInArr2d(char ** arr2d) {
		for (int i = 0; i < lenArrOfPoint; i++) {
			arr2d[pos.x + currentArrOfPoint[i].x][pos.y + currentArrOfPoint[i].y] = c;
		}		
	}
	int GetlenArrOfOrientation() { return lenArrOfOrientation; }
	char GetChar() { return c; }
	~Brick() {
		for (int i = 0; i < lenArrOfOrientation; i++) {
			delete[] allArrOfPoint[i];
		}
		delete[] allArrOfPoint;
		delete[] arrIndexOfMinXYPoint;
	}
};
class Board {
private :
	char **arr2d;//obszar na ktory program bedzie klasc klocki
	list<Brick*> * list;//lista wszyszkich klockow
	std::list<Brick*>  listBrickOnBoard;
	clock_t startTime;
	float lastTime;
	float totalTime;
	int counterCombination;
	const int a;//szerokosc
	const int b;//wysokosc
	int numberOfCombination;
	char **savedArr2d;//ostanie ustawienie
	vector<char**> oldCombination;//poprzednia kombinacja
	Point FindPointToCover(Point p) {
		for (int i = p.y; i < b; i++) {
			for (int j = 0; j < a; j++) {
				if (arr2d[j][i] == 0) {
					p.x = j;
					p.y = i;
					return p;
				}
			}
		}
		return p;
	}
	void Show() {
		for (int i = 0; i < b; i++) {
			cout << endl;
			for (int j = 0; j < a; j++) {
				if(savedArr2d[j][i]!=0)
				cout << savedArr2d[j][i];
				else cout << " ";
			}			
		}
		cout << endl << "-------------";
	}
	void SaveArr2d(){
		char **arr2d2 = new char *[a];
		for (int i = 0; i < a; i++) arr2d2[i] = new char[b];
		for (int i = 0; i < b; i++) 
			for (int j = 0; j < a; j++)
				arr2d2[j][i]=arr2d[j][i];
		savedArr2d = arr2d2;
		oldCombination.push_back(arr2d2);
	}
	void EraseLatestBrick() {
		Brick * brick = listBrickOnBoard.back();
		brick->TakeFromArr2d(arr2d);
		listBrickOnBoard.pop_back();
		list->push_front(brick);
	}
	//sprawdza czy po wstawaniu klocka nie stworzylismy dziury 
	bool IsThereHole(Point p) {
		int index = 0;
		int sum = 1;
		int x, y;
		Point arr5Point[5];
		arr5Point[0] = p;
		arr2d[arr5Point[0].x][arr5Point[0].y] = 2;
		while (index <sum) {
			x = arr5Point[index].x;
			y = arr5Point[index].y;
			x++;
			if (x<a&& arr2d[x][y]==0 ) {
				arr2d[x][y] = 2;
				arr5Point[sum].x = x;
				arr5Point[sum].y = y;
				sum++;
				if(sum==5)
					return ClearArr2dAndReturnBool(arr5Point, 5, false);
			}
			x -= 2;
			if (x>=0&& arr2d[x][y] == 0) {
				arr2d[x][y] = 2;
				arr5Point[sum].x = x;
				arr5Point[sum].y = y;
				sum++;
				if (sum == 5)
					return ClearArr2dAndReturnBool(arr5Point, 5, false);
			}
			x++;
			y++;
			if (y<b&& arr2d[x][y] == 0) {
				arr2d[x][y] = 2;
				arr5Point[sum].x = x;
				arr5Point[sum].y = y;
				sum++;
				if (sum == 5)
					return ClearArr2dAndReturnBool(arr5Point, 5, false);
			}
			y -= 2;
			if (y>=0&& arr2d[x][y] == 0) {
				arr2d[x][y] = 2;
				arr5Point[sum].x = x;
				arr5Point[sum].y = y;
				sum++;
				if (sum == 5)
					return ClearArr2dAndReturnBool(arr5Point, 5, false);
			}
			index++;
		}
		return  ClearArr2dAndReturnBool(arr5Point,sum,true);
	}
	bool ClearArr2dAndReturnBool(Point * arr5,int len ,bool bReturn) {
		for (int i = 0; i < len; i++) 
			arr2d[arr5[i].x][arr5[i].y] = 0;
		return bReturn;
	}
	bool CheckIsCombinationNew() {
		for (int i = 0; i < oldCombination.size(); i++) {
			if (CheckIsCombinationTheSame(oldCombination[i])) return false;
		}
		return true;
	}
	// sprawdza czy najnowsze ulozenie nie jest jakims poprzednim ulozenium otrzymanym przez obrocenie czy  przewrocenie
	bool CheckIsCombinationTheSame(char **arr2dOld) {
		bool same = true;
		for (int i = 0; i < a; i++) {
			for (int j = 0; j < b; j++) {
				if (arr2dOld[i][j] != arr2d[i][j]) {
					same = false;
					break;
				}
			}
			if (!same)break;
		}
		if (same)return true;
		else same = true;
		for (int i = a-1; i >=0; i--) {
			for (int j = 0; j < b; j++) {
				if (arr2dOld[i][j] != arr2d[a-1-i][j]) {
					same = false;
					break;
				}
			}
			if (!same)break;
		}
		if (same)return true;
		else same = true;
		for (int i = 0; i < a; i++) {
			for (int j = b-1; j>=0; j--) {
				if (arr2dOld[i][j] != arr2d[i][b-1-j]) {
					same = false;
					break;
				}
			}
			if (!same)break;
		}
		if (same)return true;
		else same = true;
		for (int i = a-1; i>=0; i--) {
			for (int j = b-1; j>=0; j--) {
				if (arr2dOld[i][j] != arr2d[a-1-i][b-1-j]) {
					same = false;
					break;
				}
			}
			if (!same)break;
		}
		if (same)return true;
		else return false;
	}
public:
	Board(int _a, int _b,std::list<Brick*> * _list, int _numberOfCombination) : a(_a) , b(_b) , numberOfCombination(_numberOfCombination) {
		startTime = clock();
		list = _list;
		arr2d = new char*[a];
		savedArr2d = new char*[a];
		for (int i = 0; i < a; i++) {
			arr2d[i] = new char[b];
			savedArr2d[i] = new char[b];
		}
		for (int i = 0; i < a; i++) {
			for (int j = 0; j < b; j++) {
				arr2d[i][j] = 0;
			}
		}
		totalTime = 0;
		counterCombination = 0;
	}
	void Build() { 
		Point p;
		Brick *Xbrick=list->front();
		list->pop_front();
		//klocek X ktora ma tylko jeden sposob zorientowania ustawiamian na wszystkich mozliwych miejscach w cwiartce 
		//pierwszej , to pomoze usunac sporo starych ustawien
		for(int x =0; x <=a/2;x++) {
			if (x + 2 >= a)continue;
			for(int y=0; y <=b/2; y++) {
				Xbrick->SetPos(x,y);
				Xbrick->PutInArr2d(arr2d);
				p.x = 0; p.y = 0;
				Build(p);
				Xbrick->TakeFromArr2d(arr2d);
			}			
		}
		if(numberOfCombination >counterCombination) {
			Show();
			ShowTime();
		}
	}
	void Build( Point p) {// cele tej funkcji ten pokryc kwadrat 1X1 (Point p)		
		Point possiblePoint;
		int nOrient;
		//ShowCurrentStateOnConsole();
		if (listBrickOnBoard.size() == 11) {
			//gdy juz sie  ulozy w prostokat
			if (CheckIsCombinationNew()) {
				counterCombination++;
				SaveArr2d();
				if (counterCombination == numberOfCombination) {
					Show();
					ShowTime();
					cout << endl << "Podaj nastepny number ulozenia: ";
					cin >> numberOfCombination;
					startTime = clock();
				}
			}
			EraseLatestBrick();
			return;
		}
		Brick * brick = list->front();
		nOrient = brick->GetlenArrOfOrientation();
		Brick * FirstBrick = brick;
		int iOrient;
		do {
			iOrient = 0;
			do {
				if (brick->IsMatch(arr2d, a, b, p)) {
					brick->PutInArr2d(arr2d);
					possiblePoint = FindPointToCover(p);
					if (listBrickOnBoard.size()>=10 || !IsThereHole(possiblePoint)) {
						listBrickOnBoard.push_back(brick);
						list->pop_front();
						Build(possiblePoint);//klocek pasowal i nie robil dziure
					} 
					else brick->TakeFromArr2d(arr2d);//klocek stwarzal drziure	
				}
				iOrient++;
				brick->NextPosition();//klocek nie pasowal wiec prubuje sprawdzic nastepna orientacje
			} while (iOrient < nOrient);
			list->pop_front();///kazde orientacja danego klocka nie pasowala wiec wedruje na koniec listy 
			list->push_back(brick);
			brick = list->front();
			nOrient = brick->GetlenArrOfOrientation();
		} while (FirstBrick != brick);

		if(listBrickOnBoard.size()!=0)
			EraseLatestBrick();
	}
	void ShowTime() {
		lastTime = float(clock() - startTime) / CLOCKS_PER_SEC;
		totalTime += lastTime;
		cout << endl << lastTime;
	}
	void ShowResultOfBuild() {
		cout << endl << a << "x" << b << " " <<" wszystkie ulozenia: "<< counterCombination;		
	}
	void ShowCurrentStateOnConsole() {
		system("cls");
		Show();
		Sleep(1);
	}
	~Board() {
		char **arr2dToDeleted;
		while(oldCombination.size()>0){
			arr2dToDeleted = oldCombination.back();
			oldCombination.pop_back();
			for (int i = 0; i < a; i++)delete[] arr2dToDeleted[i];
			delete[] arr2dToDeleted;
		}
		for (int i = 0; i < a; i++) {
			delete[] arr2d[i];
			arr2d[i] = nullptr;
		}
		delete[] arr2d;
		arr2d = nullptr;
	}
};


int main() {
	int end;
	//zapisanie wszystkich klockow do pamieci
	list<Brick*> *listOfBrick = new list<Brick*>();
	Point * p = new Point[5];
	Brick *b;
	bool * arr = new bool[1]{ 0 };
	p[0].x = 1; p[0].y = 0; p[1].x = 0; p[1].y = 1; p[2].x = 1; p[2].y = 1; p[3].x = 2; p[3].y = 1; p[4].x = 1; p[4].y = 2;//X 
	b = new Brick(p, 1,arr, 'X');
	listOfBrick->push_back(b);
	p = new Point[5];
	arr = new bool[2]{ 0,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 0; p[1].y = 1; p[2].x = 0; p[2].y = 2; p[3].x = 0; p[3].y = 3; p[4].x = 0; p[4].y = 4;//I
	b = new Brick(p, 2,arr, 'I');
	listOfBrick->push_back(b);
	p = new Point[5];
	arr = new bool[4]{ 1,0,1,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 1; p[1].y = 0; p[2].x = 1; p[2].y = 1; p[3].x = 1; p[3].y = 2; p[4].x = 2; p[4].y = 2;//z
	b = new Brick(p,4 ,arr, 'Z');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[4]{ 0,0,0,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 1; p[1].y = 0; p[2].x = 2; p[2].y = 0; p[3].x = 0; p[3].y = 1; p[4].x = 0; p[4].y = 2;//v
	b = new Brick(p,4, arr, 'V');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[4]{0,0,0,0 };
	p[0].x = 1; p[0].y = 0; p[1].x = 2; p[1].y = 0; p[2].x = 0; p[2].y = 1; p[3].x = 1; p[3].y = 1; p[4].x = 0; p[4].y = 2;//w
	b = new Brick(p,4, arr,'W');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[4]{0,0,0,0};
	p[0].x = 0; p[0].y = 0; p[1].x = 1; p[1].y = 0; p[2].x = 2; p[2].y = 0; p[3].x = 1; p[3].y = 1; p[4].x = 1; p[4].y = 2;//T
	b = new Brick(p,4 ,arr,'T');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[4]{0,0,0,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 2; p[1].y = 0; p[2].x = 0; p[2].y = 1; p[3].x = 1; p[3].y = 1; p[4].x = 2; p[4].y = 1;//U
	b = new Brick(p,4, arr,'U');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[8]{ 1,0,0,0,1,0,0,0 };
	p[0].x = 1; p[0].y = 0; p[1].x = 2; p[1].y = 0; p[2].x = 0; p[2].y = 1; p[3].x = 1; p[3].y = 1; p[4].x = 1; p[4].y = 2;//f
	b = new Brick(p, 8,arr,'F');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[8]{ 1,0,0,0,1,0,0,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 0; p[1].y = 1; p[2].x = 0; p[2].y = 2; p[3].x = 0; p[3].y = 3; p[4].x = 1; p[4].y = 3;//l
	b = new Brick(p,8 ,arr,'L');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[8]{ 1,0,0,0,1,0,0,0 };
	p[0].x = 1; p[0].y = 0; p[1].x = 1; p[1].y = 1; p[2].x = 1; p[2].y = 2; p[3].x = 0; p[3].y = 2; p[4].x = 0; p[4].y = 3;//N
	b = new Brick(p,8, arr,'N');
	listOfBrick->push_back(b);
	p = new Point[5];
	arr = new bool[8]{ 1,0,0,0,1,0,0,0 };
	p[0].x = 0; p[0].y = 0; p[1].x = 1; p[1].y = 0; p[2].x = 0; p[2].y = 1; p[3].x = 1; p[3].y = 1; p[4].x = 0; p[4].y = 2;//P
	b = new Brick(p,8, arr,'P');
	listOfBrick->push_back(b);	p = new Point[5];
	arr = new bool[8]{ 1,0,0,0,1,0,0,0 };
	p[0].x = 1; p[0].y = 0; p[1].x = 0; p[1].y = 1; p[2].x = 1; p[2].y = 1; p[3].x = 1; p[3].y = 2; p[4].x = 1; p[4].y = 3;//Y
	b = new Brick(p, 8,arr,'Y');
	listOfBrick->push_back(b);
		
	cout<<"Podaj wymiary: "<<endl;
	int width, height;
	cin>>width;
	cin>>height;
	cout<<"podaj numer ulozenia: "<<endl;
	int n;
	cin>>n;
	Board * board= new Board( width , height , listOfBrick,n);
	board->Build();
	board->ShowResultOfBuild();
	delete board; board = nullptr;
	delete listOfBrick;
	cin>>end;
	return 0;
}

