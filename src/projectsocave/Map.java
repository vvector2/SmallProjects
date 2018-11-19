/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projectsocave;

import java.util.Random;

/**
 *
 * @author vecto
 */
////opis pol
//0 - puste pole
//1 - sciana
//2 - turysta
//3 - wejscie
//4 - wyjscie

public class Map {
    private int nIn;
    private int nOut;
    public int[][]arr;
    private Random generator;
    public Point []arrOfEntry;
    public Map(int numberOfInput, int numberOfOutput){
        nIn=numberOfInput;
        nOut=numberOfOutput;
        Point helpPoint;
        int helpInt;
        arr=new int[10][10];
        arrOfEntry = new Point[nIn];
        ///generator
        generator = new Random();
        for(int i=0; i < nOut ;i++){
            helpPoint = findRandomPosOnBorder();
            helpInt= arr[helpPoint.x][helpPoint.y];
            if(helpInt==4){
                //szuka drugi raz
                i--;
            }else {
                arr[helpPoint.x][helpPoint.y]=4;
            }
        }
        //tworze ilosc wejsc
        for(int i=0; i < nIn ;i++){
            helpPoint = findRandomPosOnBorder();
            helpInt= arr[helpPoint.x][helpPoint.y];
            if(helpInt==4 || helpInt==3 ){
                //szuka drugi raz
                i--;
            }else {
                arr[helpPoint.x][helpPoint.y]=3;
                arrOfEntry[i]=helpPoint;
            }
        }
        fillBorder();
        fillTheRest();
        
    }
    private void fillTheRest(){
        //dol
        int random;
        for(int i =2; i < 8 ; i++){
            if(arr[i][0]!=3 &&arr[i][0]!=4 ){
                random=getRandomInRange(0,3);
                for(int j=0 ; j < random ; j++){
                    if(arr[i][j+1]==0){
                        arr[i][j+1]=1;
                    }else break;
                }
            }
        }
        ///gora
        for(int i =2; i < 8 ; i++){
            if(arr[i][9]!=3 &&arr[i][9]!=4 ){
                random=getRandomInRange(0,3);
                for(int j=0 ; j <random ; j++){
                    if(arr[i][8-j]==0){
                        arr[i][8-j]=1;
                    }else break;
                }
            }
        }
        ///lewo
        for(int j =2; j < 8 ; j++){
            if(arr[0][j]!=3 &&arr[0][j]!=4 ){
                random=getRandomInRange(0,3);
                for(int i=0 ; i <random ; i++){
                    if(arr[i+1][j]==0 && arr[i+2][j+1]==0 && arr[i+2][j-1]==0){
                        arr[i+1][j]=1;
                    }else break;
                }
            }
        }
        //prawo
        for(int j =2; j < 8 ; j++){
            if(arr[9][j]!=3 &&arr[9][j]!=4 ){
                random=getRandomInRange(0,3);
                for(int i=0 ; i <random ; i++){
                    if(arr[8-i][j]==0 && arr[7-i][j+1]==0 && arr[7-i][j-1]==0){
                        arr[8-i][j]=1;
                    }else break;
                }
            }
        }
    }
    private int getRandomInRange( int start, int count ){
        int a = generator.nextInt()%count;
        a= (a<0) ? -a:a;
        a+=start;
        return a;
    }
    private Point findRandomPosOnBorder( ){
        int a = getRandomInRange(0,4);
        Point result;
        if(a==0){
            a= getRandomInRange(1,8);
            result = new Point (a, 9);
        }else if(a==1) {
            a= getRandomInRange(1,8);
            result = new Point (a, 0);
        } else if(a==2){
            a= getRandomInRange(1,8);
            result = new Point (0, a);
        } else {
            a= getRandomInRange(1,8);
            result = new Point (9, a);
        }
        return result;
    }
    private void fillBorder(){
        for(int i =0; i < 10 ;i++){
            if(arr[i][0]==0 )arr[i][0]=1;
        }
        for(int i =0; i < 10 ;i++){
            if(arr[i][9]==0 )arr[i][9]=1;
        }
        for(int i =0; i < 10 ;i++){
            if(arr[0][i]==0 )arr[0][i]=1;
        }
        for(int i =0; i < 10 ;i++){
            if(arr[9][i]==0 )arr[9][i]=1;
        }
    }
    
}
class Point {
    public int x;
    public int y;
    public Point(){
        
    }
    public Point ( Point a ){
        this.x=a.x;
        this.y=a.y;
    }
    public Point ( int a , int b ){
        this.x=a;
        this.y=b;
    }
    public void copy (Point a){
        this.x=a.x;
        this.y=a.y;
    }
}