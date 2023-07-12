
package Modelos;

import java.util.Date;


public class Block {
    private int index;
    private String date;
    private String data;
    private int nonce;
    private String prevHash;
    private String hash;

    public Block(int index, String date, String data, int nonce, String prevHash, String hash) {
        this.index = index;
        this.date = date;
        this.data = data;
        this.nonce = nonce;
        this.prevHash = prevHash;
        this.hash = hash;
    }

    public int getIndex() {
        return index;
    }

    public void setIndex(int index) {
        this.index = index;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public int getNonce() {
        return nonce;
    }

    public void setNonce(int nonce) {
        this.nonce = nonce;
    }

    public String getPrevHash() {
        return prevHash;
    }

    public void setPrevHash(String prevHash) {
        this.prevHash = prevHash;
    }

    public String getHash() {
        return hash;
    }

    public void setHash(String hash) {
        this.hash = hash;
    }
    
    
    
}
