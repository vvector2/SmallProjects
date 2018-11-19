/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projectsocave;

import java.util.Random;
import java.util.concurrent.Semaphore;
import static projectsocave.Window.nTouristFindingExit;

/**
 *
 * @author vecto
 */
public class Tourist implements Runnable {
    private Thread thread;//watek
    public Point pos;
    private Map map;
    private Semaphore[][] accessToMap;
    boolean firstMove;
    enum Direction {
        Up,Left,Down, Right
    }
    public Tourist(Point _pos,Map _map, Semaphore[][] sem){
        pos = _pos;
        map = _map;
        accessToMap=sem;
        firstMove=true;
    }
    public void start(){
        thread = new Thread(this);
        thread.start();
    }
    public void sleep(long x) throws InterruptedException {
        Thread.sleep(x);
    }
    /*
    Pomysl na algorytm:
    Zakładam ,że turysta nie wie jak wyglada mapa i wie tylko co znajduje
    na polach które stykaja z jego polem..
    Ludzik bedzie sie poruszal zaleznie od tego gdzie jest położona ściana o którą
    sie opiera.
    */
    public void run(){
        Point wall= new Point();//sciana o ktora bedzie sie opieral ludzik
        Direction curDir;//kierunek w ktorym pojdzie turysta w nastepnym razem
        accessToMap[pos.x][pos.y].acquireUninterruptibly();
        ///okreslenie sciany o ktora bedzie sie opieral
        if(pos.x==0){
            wall.x=pos.x;
            wall.y=pos.y-1;
        }else if( pos.x==9){
            wall.x=pos.x;
            wall.y=pos.y+1;
        }else if(pos.y==0){
            wall.x=pos.x+1;
            wall.y=pos.y;
        }else {
            wall.x=pos.x-1;
            wall.y=pos.y;
        }
        do{
            //rozejrzenie sie i ewentualna zmiana scina o ktora bedzie sie opieralludzi
            if(wall.x==pos.x || wall.y == pos.y){
                ///ludzi styka sie ze scian calym bokiem
                if(wall.y==pos.y){
                    ///ludzik ma scian albo na lewo albo na prawo
                    if(wall.x+1 == pos.x){
                        //ludzi ma sciane na lewo
                        curDir=Direction.Down;
                    }else{
                        curDir=Direction.Up;
                    }
                }else {
                    if(wall.y+1==pos.y){
                        curDir=Direction.Right;
                    }else {
                        curDir=Direction.Left;
                    }
                }
                overwriteWall(wall,curDir);
            }else {
                //ludzi styka sie ze sciana tylko wierzcholkiem
                if(wall.x+1 == pos.x && wall.y+1==pos.y){
                    curDir=Direction.Down;
                }else if(wall.x-1 == pos.x && wall.y-1==pos.y){
                    curDir=Direction.Up;
                }else if(wall.x+1 == pos.x && wall.y-1==pos.y){
                    curDir=Direction.Left;
                }else {
                    curDir=Direction.Right;
                }
            }
            ///jesli turysta zamierza pojsc tam gdzie jest sciana to nie idzie 
            ///tam tylko zaczyna sie opierac o ta sciane
            if(checkIfThereIsWall(curDir, wall))continue;

            makeMove(curDir);

            //turysci odpoczywaja
            try{
            this.sleep( getRandomInRange(100,900)) ;}
            catch(InterruptedException e){
                return;
            }
        }while(!checkExit());
        map.arr[pos.x][pos.y]=0;
        accessToMap[pos.x][pos.y].release();
        nTouristFindingExit--;       
    }
    private boolean checkExit(){
        boolean result =false;
        if(map.arr[pos.x+1][pos.y]==4)result=true;
        if(map.arr[pos.x][pos.y+1]==4)result=true;
        if(map.arr[pos.x-1][pos.y]==4)result=true;
        if(map.arr[pos.x][pos.y-1]==4)result=true;
        return result;
    }
    private boolean checkIfThereIsWall(Direction dir, Point wall){
        Point helpPoint = new Point(pos);
        boolean result = false;
        changePoint(dir, helpPoint);
        if(map.arr[helpPoint.x][helpPoint.y]==1||
                map.arr[helpPoint.x][helpPoint.y]==3){
            result=true;
            wall.copy(helpPoint);
        }
        return result;
    }
    private int getRandomInRange( int start, int count ){
        Random generator = new Random(0);
        int a = generator.nextInt()%count+start;
        return (a<0) ? -a:a;
    }    
    ///zmienia punkt wzgledem kierunku
    private void changePoint(Direction curDir, Point position){
        if(curDir == Direction.Up){
            position.y++;
        }else if(curDir == Direction.Right){
            position.x++;
        }else if(curDir == Direction.Down){
            position.y--;
        }else {
            position.x--;
        }
    }
    ///funckja ktora zmienia sciane o ktora sie opiera
    public void overwriteWall(Point wall, Direction dir){
        Point helpPoint = new Point(wall);
        changePoint(dir,helpPoint);
        if(map.arr[helpPoint.x][helpPoint.y]==1 ||
                map.arr[helpPoint.x][helpPoint.y]==3){
            wall.copy(helpPoint);
        }
    }
    ///zaznacza odpowiednio zmianny na mapie
    private void makeMove(Direction curDir){
        if(!firstMove)map.arr[pos.x][pos.y]=0;
        else firstMove=false;
        accessToMap[pos.x][pos.y].release();
        changePoint(curDir,pos);
        accessToMap[pos.x][pos.y].acquireUninterruptibly();///w tym momencie
        ///proces moze zostac zatrzymany
        map.arr[pos.x][pos.y]=2;
    }
}