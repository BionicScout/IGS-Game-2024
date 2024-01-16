using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexLayout : MonoBehaviour {

    //struct Orientation {
    //    public double f0, f1, f2, f3;
    //    public double b0, b1, b2, b3;
    //    public double start_angle; // in multiples of 60°
    //    public Orientation(double f0_ , double f1_ , double f2_ , double f3_ ,
    //                        double b0_ , double b1_ , double b2_ , double b3_ ,
    //                        double start_angle_) {
    //        f0 = f0_;
    //        f1 = f1_;
    //        f2 = f2_;
    //        f3 = f3_;
    //        b0 = b0_;
    //        b1 = b1_;
    //        b2 = b2_;
    //        b3 = b3_;
    //        start_angle = start_angle_;
    //    }

    //};

    //Orientation layout_pointy =
    //    new Orientation(Mathf.Sqrt(3.0f) , Mathf.Sqrt(3.0f) / 2.0 , 0.0 , 3.0 / 2.0 ,
    //                    Mathf.Sqrt(3.0f) / 3.0 , -1.0 / 3.0 , 0.0 , 2.0 / 3.0 ,
    //                    0.5);

    //struct Layout {
    //    public Orientation orientation;
    //    public Point size;
    //    public Point origin;
    //    public Layout(Orientation orientation_ , Point size_ , Point origin_) {
    //        orientation = orientation_;
    //        size = size_;
    //        origin = origin_;
    //    }
    //};

    //struct Point {
    //    public double x, y;
    //    public Point(double x_ , double y_) {
    //        x = x_;
    //        y = y_;
    //    }
    //};

    //Point hex_to_pixel(Layout layout , Hex h) {
    //    Orientation op = layout.orientation;
    //    double x = (op.f0 * h.q + op.f1 * h.r) * layout.size.x;
    //    double y = (op.f2 * h.q + op.f3 * h.r) * layout.size.y;
    //    return new Point(x + layout.origin.x , y + layout.origin.y);
    //}

    ////public FractionalHex pixel_to_hex(Layout layout , Point p) {
    ////{
    ////    const Orientation op = layout.orientation;
    ////    Point pt = Point((p.x - layout.origin.x) / layout.size.x ,
    ////                        (p.y - layout.origin.y) / layout.size.y);
    ////    double q = op.b0 * pt.x + op.b1 * pt.y;
    ////    double r = op.b2 * pt.x + op.b3 * pt.y;
    ////    return FractionalHex(q , r , -q - r);
    ////}



    //Point hex_corner_offset(Layout layout , int corner) {
    //    Point size = layout.size;
    //    float angle = (float)(2.0 * Mathf.PI * (layout.orientation.start_angle + corner) / 6);
    //    return new Point(size.x * Mathf.Cos(angle) , size.y * Mathf.Sin(angle));
    //}

    //List<Point> polygon_corners(Layout layout , Hex h) {
    //    List<Point> corners = new List<Point>{ };
    //    Point center = hex_to_pixel(layout , h);

    //    for(int i = 0; i < 6; i++) {
    //        Point offset = hex_corner_offset(layout , i);
    //        corners.Add(new Point(center.x + offset.x , center.y + offset.y));
    //    }
    //    return corners;
    //}



    public GameObject Testagon;
    public Vector2Int size;

    public Sprite clickSprite;

    private void Start() {
        Vector2 spacing = new Vector2(Mathf.Sqrt(3) * size.x, 1.5f * size.y);
        Hex center = new Hex(-1, -1, -1);

        //All Tiles
        List<Hex> map = new List<Hex>();
        for(int q = -7; q <= 4; q++) {
            for(int r = -3; r <= 3; r++) {
                Hex h = new Hex(q , r , -q - r);
                map.Add(h);

                GameObject obj = Instantiate(Testagon);
                obj.name = q + " " + r + " " + (-q - r);

                Vector2 pos = new Vector2(spacing.x * q , spacing.y * r) * 0.5f;
                Vector2 offset = new Vector2(r *  spacing.x * 0.25f, 0);
                obj.transform.position = pos + offset;
                //Debug.Log((Mathf.Sqrt(3) * size.x * q) + "     " + q);

                obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "-1";

                h.attachedObj = obj;


                if(q == 0 && r == 0) 
                    center = h;
            }
        }

        Debug.Log(center.attachedObj);



    //Find All neighnors
        for(int i = 0; i < map.Count; i++) {
            string s = map[i].hex_length(map[i]).ToString();
            map[i].attachedObj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = s;
        }
    }

    public void adjNeighbor(Hex hex) {
        for(int i = 0; i < 6;  i++) {
            Hex neighboor = hex.hex_neighbor(hex, i);

            if(neighboor.attachedObj == null)
                return;
            

        }
    }

    public void Update() {
        //if(Input.GetMouseButtonDown(0)) {
        //    mousePosition = Input.mousePosition;

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit2D hit = Physics2D.Raycast(ray.origin , ray.direction);
        //}
    }

}
