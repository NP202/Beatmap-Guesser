public class Image {

    public String filepath {get; private set;}
    public int height {get; private set;}
    public int width {get; private set;}

    public Image(String filepath, int height, int width)
    {
        this.filepath = filepath;
        this.height = height;
        this.width = width;
       
    }


    
}