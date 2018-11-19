/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projectsocave;

import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.Semaphore;
import javax.imageio.ImageIO;
import javax.swing.JButton;
import static projectsocave.Window.nTouristFindingExit;

/**
 *
 * @author vecto
 */
public class Render implements Runnable {
    private Thread thread;
    private Map map;
    Semaphore[][] accessToMap;
    private final int timeSleep;
    private final int fieldBoxLen;
    private Graphics2D gfx;
    private boolean lastDraw;
    private JButton btn;
    BufferedImage imgWall;
    BufferedImage imgTourist;
    BufferedImage imgExit;
    BufferedImage imgEntry;
    BufferedImage imgEmpty;
    private BufferedImage loadImage(String src){
        File f;
        BufferedImage img;
        try {
            f= new File( "src\\projectsocave\\"+ src );
            img =new BufferedImage(30,30, BufferedImage.TYPE_INT_ARGB);
            img= ImageIO.read(f);
        } catch (IOException e) {
            img=null;
        }
        return img;
    }
    public Render(Map _map , Semaphore[][] sem, Graphics2D _gfx, JButton _btn){
        map= _map;
        accessToMap= sem;
        btn=_btn;
        fieldBoxLen = 30;
        gfx = _gfx;
        timeSleep=100;
        imgWall =loadImage("wall.png");
        imgTourist =loadImage("man.png");
        imgExit =loadImage("exit.png");
        imgEntry =loadImage("entry.png");
        imgEmpty = loadImage("empty.png");
        lastDraw=false;
    }
    public void start(){
        thread= new Thread(this);
        thread.start();
    }
    public void sleep(long x) throws InterruptedException {
        Thread.sleep(x);
    }
    public void run(){
        while(!lastDraw){
            if(gfx==null)return;
            if(nTouristFindingExit==0)lastDraw=true;
            ///renderuje mapke 
            for(int i=0; i < 10 ;i++) {
                for(int j=0; j < 10 ; j++) {
                    if(map.arr[i][j]==0){
                        gfx.drawImage(imgEmpty, i*fieldBoxLen, 
                                270-j*fieldBoxLen, null);                        
                    }
                    else if(map.arr[i][j]==1){
                        gfx.drawImage(imgWall, i*fieldBoxLen, 
                                270-j*fieldBoxLen, null);
                    }else if(map.arr[i][j]==2){
                        gfx.drawImage(imgTourist, i*fieldBoxLen, 
                                270-j*fieldBoxLen, null);                       
                    }else if(map.arr[i][j]==3){
                        gfx.drawImage(imgEntry, i*fieldBoxLen, 
                                270-j*fieldBoxLen, null);                       
                    }else if(map.arr[i][j]==4){
                        gfx.drawImage(imgExit, i*fieldBoxLen, 
                                270-j*fieldBoxLen, null);                       
                    }
                }
            }
            try{this.sleep(timeSleep);
            }catch (InterruptedException e){
                return;
            }
        }
        btn.setText("Start");
    }
}
