using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chekers2
{
    public partial class Form1 : Form
    {
       int startx = -1;
       int starty = -1; 
        public Form1()
        {
            InitializeComponent();
        }
       public class node
        {
           public int[][] matrix;
           public List<node> childeren;
           public node parent;
           public int fx;
           public int fy;
           public int lx;
           public int ly;
           public int kx=-1;
           public int ky;
           public int mark=-20;

            
        }
       public class mohre
        {
          public int kind;

          public int x;
          public int y;
          public bool live = true;
            

        }public mohre[] m1, m2;
        public class tree
        {
           public node root;
            public tree()
            {
                root = new node();

            }
            public void add(node par,node newnode)
            {
                if (par == null)
                {
                    root.childeren.Add(newnode);

                }
                else
                {
                    par.childeren.Add(newnode);
                }

            }
        }
        int[][] matrix;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            m1 = new mohre[12];m2 = new mohre[12];
            for (int i = 0; i < 12; i++)
            {
                m1[i] = new mohre(); m2[i] = new mohre();
            }
            
            
            matrix = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                matrix[i] = new int[8];
            }
            int j=0;
            for (int i = 0; i < 3; i+=2)
            {
                matrix[i][1] = matrix[i][3] = matrix[i][ 5]=matrix[i][7]=1;
            }
            for (int i = 5; i < 8; i += 2)
            {
                matrix[i][0] = matrix[i][2] = matrix[i][4] = matrix[i][6] = 2;
            }
           
                matrix[1][0] = matrix[1][2] = matrix[1][4] = matrix[1][6] = 1;
                matrix[6][1] = matrix[6][3] = matrix[6][5] = matrix[6][7] = 2;
            int k1=0;
            int k2 = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int i2 = 0; i2 < 8; i2++)
                    {
                        if (matrix[i][i2] == 1)
                        {
                            m1[k1].x = i2;
                            m1[k1].y = i;
                            m1[k1].live = true;
                            m1[k1].kind = 1;
                            k1++;
                        }
                        if (matrix[i][i2] == 2)
                        {
                            m2[k2].x = i2;
                            m2[k2].y = i;
                            m2[k2].live = true;
                            m2[k2].kind = 2;
                            k2++;
                        }

                    }
                    
                }
                redraw();
                
                
        }
       public tree temptree;
        public void play()
        {
            temptree = new tree();
            List<node> l1;
            temptree.root.matrix = matrix;
            l1 = findplays(2,temptree.root);
            List<node> l2=new List<node>();
            List<node> l3=new List<node>();
            List<node> l4 = new List<node>();
            foreach(node value in l1)
            {
                List<node> templ = findplays(1, value);
                foreach (node val in templ)
                {
                    l2.Add(val);
                }
            }
            foreach (node value in l2)
            {
                List<node> templ = findplays(2, value);
                foreach (node val in templ)
                {
                    l3.Add(val);
                }
            }
            foreach (node value in l3)
            {
                List<node> templ = findplays(1, value);
                foreach (node val in templ)
                {
                    l4.Add(val);
                }
            }

           findmark(1, l4);
           findmark(2, l3);
           findmark(1, l2);
            findmark(2, l1);
            foreach (node item in l1)
            {
                if (item.mark == temptree.root.mark)
                {
                    matrix = item.matrix;
                    mohre mtemp = findmohre(item.fy,item.fx );
                    mtemp.x = item.lx;
                    mtemp.y = item.ly;
                    if (item.kx != -1)
                    {
                        mtemp = findmohre(item.ky,item.kx );
                        mtemp.live = false;
                        mtemp.x = -1;
                        mtemp.y = -1;
                    }
                    break;
                }
            }
            redraw();

        }
        public void finder(node n)
        {
            int max=0;
            int min=0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                   int val= n.matrix[i][j];
                  if (val == 1)
                    min++;
                if (val == 2)
                    max++;
                }
            }
            n.mark = max - min;
        }
        public void findmark(int type,List<node> l)
        {
            if (type == 1)
            {
                foreach (node val in l)
                {
                    if(val.parent.mark == -20)
                    val.parent.mark = 20;
                    if (val.mark == -20)
                        finder(val);
                    val.parent.mark = (val.mark < val.parent.mark) ? val.mark : val.parent.mark;
                        
                }
            }
            if (type == 2)
            {
                foreach (node val in l)
                {
                    if (val.mark == -20)
                        finder(val);
                    val.parent.mark = (val.mark> val.parent.mark) ? val.mark : val.parent.mark;

                }
            }
        }

        //1=min,2=max
        public List<node> findplays(int type,node nodet)
        {
            int[][] mat = nodet.matrix;
            List<node> list = new List<node>();
            node temp;
            int[][] mattemp;
            if (type == 1)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (m1[i].live == true)
                    {
                        if (m1[i].x + 1<8&&m1[i].y + 1<8&&mat[m1[i].y + 1][m1[i].x + 1] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m1[i].y + 1][m1[i].x + 1] = 1;
                            mattemp[m1[i].y][m1[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m1[i].x;
                            temp.lx = m1[i].x + 1;
                            temp.fy = m1[i].y;
                            temp.ly = m1[i].y + 1;
                            temp.kx = -1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m1[i].x - 1>-1&&m1[i].y + 1<8&&mat[m1[i].y + 1][m1[i].x - 1] == 0)
                        {

                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m1[i].y + 1][m1[i].x - 1] = 1;
                            mattemp[m1[i].y] [m1[i].x]= 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m1[i].x;
                            temp.lx = m1[i].x - 1;
                            temp.fy = m1[i].y;
                            temp.ly = m1[i].y + 1;
                            temp.kx = -1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m1[i].x +2 <8 && m1[i].y + 2 < 8 && mat[m1[i].y + 1] [m1[i].x + 1]== 2 && mat[m1[i].y + 2][m1[i].x + 2] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m1[i].y + 2][m1[i].x + 2] = 1;
                            mattemp[m1[i].y + 1][m1[i].x + 1] = 0;
                            mattemp[m1[i].y][m1[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m1[i].x;
                            temp.lx = m1[i].x + 2;
                            temp.fy = m1[i].y;
                            temp.ly = m1[i].y + 2;
                            temp.kx = m1[i].x + 1;
                            temp.ky = m1[i].y + 1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m1[i].x - 2 > -1 && m1[i].y + 2< 8 && mat[m1[i].y + 1][m1[i].x - 1] == 2 && mat[m1[i].y + 2][m1[i].x - 2] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m1[i].y + 2][m1[i].x - 2] = 1;
                            mattemp[m1[i].y + 1][m1[i].x - 1] = 0;
                            mattemp[m1[i].y][m1[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m1[i].x;
                            temp.lx = m1[i].x - 2;
                            temp.fy = m1[i].y;
                            temp.ly = m1[i].y + 2;
                            temp.kx = m1[i].x - 1;
                            temp.ky = m1[i].y + 1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }

                    }
                }
            }
            if (type == 2)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (m2[i].live == true)
                    {
                        if (m2[i].x + 1 <8 && m2[i].y-1 >-1 && mat[m2[i].y - 1][m2[i].x + 1] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m2[i].y - 1][m2[i].x + 1] = 2;
                            mattemp[m2[i].y][m2[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m2[i].x;
                            temp.lx = m2[i].x + 1;
                            temp.fy = m2[i].y;
                            temp.ly = m2[i].y - 1;
                            temp.kx = -1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m2[i].x - 1 >-1 && m2[i].y-1 > -1 && mat[m2[i].y - 1][m2[i].x - 1] == 0)
                        {

                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m2[i].y - 1][m2[i].x - 1] = 2;
                            mattemp[m2[i].y][m2[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m2[i].x;
                            temp.lx = m2[i].x - 1;
                            temp.fy = m2[i].y;
                            temp.ly = m2[i].y - 1;
                            temp.kx = -1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m2[i].x + 2 < 8 && m2[i].y-2 > -1 && mat[m2[i].y - 1][m2[i].x + 1] == 1 && mat[m2[i].y -2][m2[i].x + 2] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m2[i].y - 2][m2[i].x + 2] = 2;
                            mattemp[m2[i].y - 1][m2[i].x + 1] = 0;
                            mattemp[m2[i].y][m2[i].x] = 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m2[i].x;
                            temp.lx = m2[i].x + 2;
                            temp.fy = m2[i].y;
                            temp.ly = m2[i].y - 2;
                            temp.kx = m2[i].x + 1;
                            temp.ky = m2[i].y - 1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }
                        if (m2[i].x -2>-1 && m2[i].y - 2 > -1 && mat[m2[i].y - 1][m2[i].x - 1] == 1 && mat[m2[i].y - 2][m2[i].x - 2] == 0)
                        {
                            mattemp = new int[8][];
                            for (int item = 0; item < 8; item++)
                            {
                                mattemp[item] = new int[8];
                                mat[item].CopyTo(mattemp[item], 0);
                            }
                            mattemp[m2[i].y - 2][m2[i].x - 2] = 2;
                            mattemp[m2[i].y - 1] [m2[i].x - 1]= 0;
                            mattemp[m2[i].y] [m2[i].x]= 0;
                            temp = new node();
                            temp.parent = nodet;
                            temp.fx = m2[i].x;
                            temp.lx = m2[i].x - 2;
                            temp.fy = m2[i].y;
                            temp.ly = m2[i].y - 2;
                            temp.kx = m2[i].x - 1;
                            temp.ky = m2[i].y -1;
                            temp.matrix = mattemp;
                            list.Add(temp);
                        }

                    }
                }
            }
            nodet.childeren = list;
            return list;

        }




        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void redraw()
        {
            if (matrix[0][1] == 1)
                p01.Image = chekers2.Properties.Resources.mw;
            else if (matrix[0][1] == 2)
                p01.Image = chekers2.Properties.Resources.mb;
            else if (matrix[0][1] == 3)
                p01.Image = chekers2.Properties.Resources.mwv;
            else if(matrix[0][1]==4)
                p01.Image = chekers2.Properties.Resources.mbv;
            else
                p01.Image = chekers2.Properties.Resources.m;
            if (matrix[0][3] == 1)
                p03.Image = chekers2.Properties.Resources.mw;
            else if (matrix[0][3] == 2)
                p03.Image = chekers2.Properties.Resources.mb;
            else if (matrix[0][3] == 3)
                p03.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[0][3] == 4)
                p03.Image = chekers2.Properties.Resources.mbv;
            else
                p03.Image = chekers2.Properties.Resources.m;
            if (matrix[0][5] == 1)
                p05.Image = chekers2.Properties.Resources.mw;
            else if (matrix[0][5] == 2)
                p05.Image = chekers2.Properties.Resources.mb;
            else if (matrix[0][5] == 3)
                p05.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[0][5] == 4)
                p05.Image = chekers2.Properties.Resources.mbv;
            else
                p05.Image = chekers2.Properties.Resources.m;
            if (matrix[0][7] == 1)
                p07.Image = chekers2.Properties.Resources.mw;
            else if (matrix[0][7] == 2)
                p07.Image = chekers2.Properties.Resources.mb;
            else if (matrix[0][7] == 3)
                p07.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[0][7] == 4)
                p07.Image = chekers2.Properties.Resources.mbv;
            else
                p07.Image = chekers2.Properties.Resources.m;
            /////////////////////////////////////////////////////////////
            if (matrix[2][1] == 1)
                p21.Image = chekers2.Properties.Resources.mw;
            else if (matrix[2][1] == 2)
                p21.Image = chekers2.Properties.Resources.mb;
            else if (matrix[2][1] == 3)
                p21.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[2][1] == 4)
                p21.Image = chekers2.Properties.Resources.mbv;
            else
                p21.Image = chekers2.Properties.Resources.m;
            if (matrix[2][3] == 1)
                p23.Image = chekers2.Properties.Resources.mw;
            else if (matrix[2][3] == 2)
                p23.Image = chekers2.Properties.Resources.mb;
            else if (matrix[2][3] == 3)
                p23.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[2][3] == 4)
                p23.Image = chekers2.Properties.Resources.mbv;
            else
                p23.Image = chekers2.Properties.Resources.m;
            if (matrix[2][5] == 1)
                p25.Image = chekers2.Properties.Resources.mw;
            else if (matrix[2][5] == 2)
                p25.Image = chekers2.Properties.Resources.mb;
            else if (matrix[2][5] == 3)
                p25.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[2][5] == 4)
                p25.Image = chekers2.Properties.Resources.mbv;
            else
                p25.Image = chekers2.Properties.Resources.m;
            if (matrix[2][7] == 1)
                p27.Image = chekers2.Properties.Resources.mw;
            else if (matrix[2][7] == 2)
                p27.Image = chekers2.Properties.Resources.mb;
            else if (matrix[2][7] == 3)
                p27.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[2][7] == 4)
                p27.Image = chekers2.Properties.Resources.mbv;
            else
                p27.Image = chekers2.Properties.Resources.m;
            ////////////////////////////////////////////////////////////////////////
            if (matrix[4][1] == 1)
                p41.Image = chekers2.Properties.Resources.mw;
            else if (matrix[4][1] == 2)
                p41.Image = chekers2.Properties.Resources.mb;
            else if (matrix[4][1] == 3)
                p41.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[4][1] == 4)
                p41.Image = chekers2.Properties.Resources.mbv;
            else
                p41.Image = chekers2.Properties.Resources.m;
            if (matrix[4][3] == 1)
                p43.Image = chekers2.Properties.Resources.mw;
            else if (matrix[4][3] == 2)
                p43.Image = chekers2.Properties.Resources.mb;
            else if (matrix[4][3] == 3)
                p43.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[4][3] == 4)
                p43.Image = chekers2.Properties.Resources.mbv;
            else
                p43.Image = chekers2.Properties.Resources.m;
            if (matrix[4][5] == 1)
                p45.Image = chekers2.Properties.Resources.mw;
            else if (matrix[4][5] == 2)
                p45.Image = chekers2.Properties.Resources.mb;
            else if (matrix[4][5] == 3)
                p45.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[4][5] == 4)
                p45.Image = chekers2.Properties.Resources.mbv;
            else
                p45.Image = chekers2.Properties.Resources.m;
            if (matrix[4][7] == 1)
                p47.Image = chekers2.Properties.Resources.mw;
            else if (matrix[4][7] == 2)
                p47.Image = chekers2.Properties.Resources.mb;
            else if (matrix[4][7] == 3)
                p47.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[4][7] == 4)
                p47.Image = chekers2.Properties.Resources.mbv;
            else
                p47.Image = chekers2.Properties.Resources.m;
            //////////////////////////////////////////////////////////////////
            if (matrix[6][1] == 1)
                p61.Image = chekers2.Properties.Resources.mw;
            else if (matrix[6][1] == 2)
                p61.Image = chekers2.Properties.Resources.mb;
            else if (matrix[6][1] == 3)
                p61.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[6][1] == 4)
                p61.Image = chekers2.Properties.Resources.mbv;
            else
                p61.Image = chekers2.Properties.Resources.m;
            if (matrix[6][3] == 1)
                p63.Image = chekers2.Properties.Resources.mw;
            else if (matrix[6][3] == 2)
                p63.Image = chekers2.Properties.Resources.mb;
            else if (matrix[6][3] == 3)
                p63.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[6][3] == 4)
                p63.Image = chekers2.Properties.Resources.mbv;
            else
                p63.Image = chekers2.Properties.Resources.m;
            if (matrix[6][5] == 1)
                p65.Image = chekers2.Properties.Resources.mw;
            else if (matrix[6][5] == 2)
                p65.Image = chekers2.Properties.Resources.mb;
            else if (matrix[6][5] == 3)
                p65.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[6][5] == 4)
                p65.Image = chekers2.Properties.Resources.mbv;
            else
                p65.Image = chekers2.Properties.Resources.m;
            if (matrix[6][7] == 1)
                p67.Image = chekers2.Properties.Resources.mw;
            else if (matrix[6][7] == 2)
                p67.Image = chekers2.Properties.Resources.mb;
            else if (matrix[6][7] == 3)
                p67.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[6][7] == 4)
                p67.Image = chekers2.Properties.Resources.mbv;
            else
                p67.Image = chekers2.Properties.Resources.m;
            ////////////////////////////////////////////////////////////////////
            if (matrix[1][0] == 1)
                p10.Image = chekers2.Properties.Resources.mw;
            else if (matrix[1][0] == 2)
                p10.Image = chekers2.Properties.Resources.mb;
            else if (matrix[1][0] == 3)
                p10.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[1][0] == 4)
                p10.Image = chekers2.Properties.Resources.mbv;
            else
                p10.Image = chekers2.Properties.Resources.m;
            if (matrix[1][2] == 1)
                p12.Image = chekers2.Properties.Resources.mw;
            else if (matrix[1][2] == 2)
                p12.Image = chekers2.Properties.Resources.mb;
            else if (matrix[1][2] == 3)
                p12.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[1][2] == 4)
                p12.Image = chekers2.Properties.Resources.mbv;
            else
                p12.Image = chekers2.Properties.Resources.m;
            if (matrix[1][4] == 1)
                p14.Image = chekers2.Properties.Resources.mw;
            else if (matrix[1][4] == 2)
                p14.Image = chekers2.Properties.Resources.mb;
            else if (matrix[1][4] == 3)
                p14.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[1][4] == 4)
                p14.Image = chekers2.Properties.Resources.mbv;
            else
                p14.Image = chekers2.Properties.Resources.m;
            if (matrix[1][6] == 1)
                p16.Image = chekers2.Properties.Resources.mw;
            else if (matrix[1][6] == 2)
                p16.Image = chekers2.Properties.Resources.mb;
            else if (matrix[1][6] == 3)
                p16.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[1][6] == 4)
                p16.Image = chekers2.Properties.Resources.mbv;
            else
                p16.Image = chekers2.Properties.Resources.m;
            /////////////////////////////////////////////////////////////////////
            if (matrix[3][0] == 1)
                p30.Image = chekers2.Properties.Resources.mw;
            else if (matrix[3][0] == 2)
                p30.Image = chekers2.Properties.Resources.mb;
            else if (matrix[3][0] == 3)
                p30.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[3][0] == 4)
                p30.Image = chekers2.Properties.Resources.mbv;
            else
                p30.Image = chekers2.Properties.Resources.m;
            if (matrix[3][2] == 1)
                p32.Image = chekers2.Properties.Resources.mw;
            else if (matrix[3][2] == 2)
                p32.Image = chekers2.Properties.Resources.mb;
            else if (matrix[3][2] == 3)
                p32.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[3][2] == 4)
                p32.Image = chekers2.Properties.Resources.mbv;
            else
                p32.Image = chekers2.Properties.Resources.m;
            if (matrix[3][4] == 1)
                p34.Image = chekers2.Properties.Resources.mw;
            else if (matrix[3][4] == 2)
                p34.Image = chekers2.Properties.Resources.mb;
            else if (matrix[3][4] == 3)
                p34.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[3][4] == 4)
                p34.Image = chekers2.Properties.Resources.mbv;
            else
                p34.Image = chekers2.Properties.Resources.m;
            if (matrix[3][6] == 1)
                p36.Image = chekers2.Properties.Resources.mw;
            else if (matrix[3][6] == 2)
                p36.Image = chekers2.Properties.Resources.mb;
            else if (matrix[3][6] == 3)
                p36.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[3][6] == 4)
                p36.Image = chekers2.Properties.Resources.mbv;
            else
                p36.Image = chekers2.Properties.Resources.m;
            //////////////////////////////////////////////////
            if (matrix[5][0] == 1)
                p50.Image = chekers2.Properties.Resources.mw;
            else if (matrix[5][0] == 2)
                p50.Image = chekers2.Properties.Resources.mb;
            else if (matrix[5][0] == 3)
                p50.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[5][0] == 4)
                p50.Image = chekers2.Properties.Resources.mbv;
            else
                p50.Image = chekers2.Properties.Resources.m;
            if (matrix[5][2] == 1)
                p52.Image = chekers2.Properties.Resources.mw;
            else if (matrix[5][2] == 2)
                p52.Image = chekers2.Properties.Resources.mb;
            else if (matrix[5][2] == 3)
                p52.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[5][2] == 4)
                p52.Image = chekers2.Properties.Resources.mbv;
            else
                p52.Image = chekers2.Properties.Resources.m;
            if (matrix[5][4] == 1)
                p54.Image = chekers2.Properties.Resources.mw;
            else if (matrix[5][4] == 2)
                p54.Image = chekers2.Properties.Resources.mb;
            else if (matrix[5][4] == 3)
                p54.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[5][4] == 4)
                p54.Image = chekers2.Properties.Resources.mbv;
            else
                p54.Image = chekers2.Properties.Resources.m;
            if (matrix[5][6] == 1)
                p56.Image = chekers2.Properties.Resources.mw;
            else if (matrix[5][6] == 2)
                p56.Image = chekers2.Properties.Resources.mb;
            else if (matrix[5][6] == 3)
                p56.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[5][6] == 4)
                p56.Image = chekers2.Properties.Resources.mbv;
            else
                p56.Image = chekers2.Properties.Resources.m;
            //////////////////////////////////////////////////////////
            if (matrix[7][0] == 1)
                p70.Image = chekers2.Properties.Resources.mw;
            else if (matrix[7][0] == 2)
                p70.Image = chekers2.Properties.Resources.mb;
            else if (matrix[7][0] == 3)
                p70.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[7][0] == 4)
                p70.Image = chekers2.Properties.Resources.mbv;
            else
                p70.Image = chekers2.Properties.Resources.m;
            if (matrix[7][2] == 1)
                p72.Image = chekers2.Properties.Resources.mw;
            else if (matrix[7][2] == 2)
                p72.Image = chekers2.Properties.Resources.mb;
            else if (matrix[7][2] == 3)
                p72.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[7][2] == 4)
                p72.Image = chekers2.Properties.Resources.mbv;
            else
                p72.Image = chekers2.Properties.Resources.m;
            if (matrix[7][4] == 1)
                p74.Image = chekers2.Properties.Resources.mw;
            else if (matrix[7][4] == 2)
                p74.Image = chekers2.Properties.Resources.mb;
            else if (matrix[7][4] == 3)
                p74.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[7][4] == 4)
                p74.Image = chekers2.Properties.Resources.mbv;
            else
                p74.Image = chekers2.Properties.Resources.m;
            if (matrix[7][6] == 1)
                p76.Image = chekers2.Properties.Resources.mw;
            else if (matrix[7][6] == 2)
                p76.Image = chekers2.Properties.Resources.mb;
            else if (matrix[7][6] == 3)
                p76.Image = chekers2.Properties.Resources.mwv;
            else if (matrix[7][6] == 4)
                p76.Image = chekers2.Properties.Resources.mbv;
            else
                p76.Image = chekers2.Properties.Resources.m;

        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void p70_Click(object sender, EventArgs e)
        {
            int setx = 0;
            int sety = 7;
            human(setx, sety);
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            play();
        }

        private void p01_Click(object sender, EventArgs e)
        {
            int setx = 1;
            int sety = 0;
            human(setx, sety);
        
        }

        private void p03_Click(object sender, EventArgs e)
        {
            int setx = 3;
            int sety = 0;
            human(setx, sety);
        }

        private void p05_Click(object sender, EventArgs e)
        {
            int setx = 5;
            int sety = 0;
            human(setx, sety);
        }

        private void p07_Click(object sender, EventArgs e)
        {
            int setx = 7;
            int sety = 0;
            human(setx, sety);
        }

        private void p10_Click(object sender, EventArgs e)
        {
            int setx = 0;
            int sety = 1;
            human(setx, sety);
        }

        private void p12_Click(object sender, EventArgs e)
        {
            int setx = 2;
            int sety = 1;
            human(setx, sety);
        }

        private void p14_Click(object sender, EventArgs e)
        {
            int setx = 4;
            int sety = 1;
            human(setx, sety);
        }

        private void p16_Click(object sender, EventArgs e)
        {
            int setx = 6;
            int sety = 1;
            human(setx, sety);
        }
        mohre findmohre(int y, int x)
        {
            for (int i = 0; i < 12; i++)
            {
                if (m1[i].x == x && m1[i].y == y)
                    return m1[i];
                if (m2[i].x == x && m2[i].y == y)
                    return m2[i];
            }
            return null;
        }
        private void p21_Click(object sender, EventArgs e)
        {
            int setx=1;
            int sety=2;
            human(setx, sety);

        }

        private void p23_Click(object sender, EventArgs e)
        {
            int setx = 3;
            int sety = 2;
            human(setx, sety);
        }

        private void p25_Click(object sender, EventArgs e)
        {
            int setx = 5;
            int sety = 2;
            human(setx, sety);
        }

        private void p27_Click(object sender, EventArgs e)
        {
            int setx = 7;
            int sety = 2;
            human(setx, sety);
        }

        private void p30_Click(object sender, EventArgs e)
        {
            int setx = 0;
            int sety = 3;
            human(setx, sety);
        }

        private void p32_Click(object sender, EventArgs e)
        {
            int setx = 2;
            int sety = 3;
            human(setx, sety);
        }

        private void p34_Click(object sender, EventArgs e)
        {
            int setx = 4;
            int sety = 3;
            human(setx, sety);
        }

        private void p36_Click(object sender, EventArgs e)
        {
            int setx = 6;
            int sety = 3;
            human(setx, sety);
        }

        private void p47_Click(object sender, EventArgs e)
        {
            int setx = 7;
            int sety = 4;
            human(setx, sety);
        }

        private void p45_Click(object sender, EventArgs e)
        {
            int setx = 5;
            int sety = 4;
            human(setx, sety);
        }

        private void p43_Click(object sender, EventArgs e)
        {
            int setx = 3;
            int sety = 4;
            human(setx, sety);
        }

        private void p41_Click(object sender, EventArgs e)
        {
            int setx = 1;
            int sety = 4;
            human(setx, sety);

        }

        private void p50_Click(object sender, EventArgs e)
        {
            int setx = 0;
            int sety = 5;
            human(setx, sety);
        }

        private void p52_Click(object sender, EventArgs e)
        {
            int setx = 2;
            int sety = 5;
            human(setx, sety);
        }

        private void p54_Click(object sender, EventArgs e)
        {
            int setx = 4;
            int sety = 5;
            human(setx, sety);
        }

        private void p56_Click(object sender, EventArgs e)
        {
            int setx = 6;
            int sety = 5;
            human(setx, sety);
        }

        private void p61_Click(object sender, EventArgs e)
        {
            int setx = 1;
            int sety = 6;
            human(setx, sety);
        }

        private void p63_Click(object sender, EventArgs e)
        {
            int setx = 3;
            int sety = 6;
            human(setx, sety);
        }

        private void p65_Click(object sender, EventArgs e)
        {
            int setx = 5;
            int sety = 6;
            human(setx, sety);
        }

        private void p67_Click(object sender, EventArgs e)
        {
            int setx = 7;
            int sety = 6;
           
                human(setx, sety);
        }

        private void p72_Click(object sender, EventArgs e)
        {
            int setx = 2;
            int sety = 7;
            human(setx, sety);
        }
        public void human(int setx, int sety)
        {
            if (startx == -1)
            {
                startx = setx;
                starty = sety;

            }
            else
            {
                int distancex = setx - startx;
                int distancey = sety - starty;
                if (distancex > 1 || distancex < -1 || distancey > 1 || distancey < -1)
                {
                    MessageBox.Show("you can just move to postions in neighburhood");
                }
                else
                {
                    mohre m = findmohre(starty, startx);
                    if (m == null||m.live==false)
                        MessageBox.Show("no valid mohre");
                    else
                    {
                        mohre m2 = findmohre(sety, setx);
                        if (m2 != null&&m2.live)
                        {
                            if (m2.kind == m.kind)
                            {
                                startx = -1;
                                starty = -1;
                            }
                            else
                            {
                                int flagx = m2.x - m.x;
                                int flagy = m2.y - m.y;
                                matrix[sety + flagy][setx + flagx] = m.kind;
                                m.x = setx + flagx;
                                m.y = sety + flagy;
                                matrix[sety][setx] = 0;
                                matrix[starty][startx] = 0;
                                m2.live = false;
                                m2.x = -1;
                                m2.y = -1;

                            }
                        }
                        else
                        {
                            m.x = setx;
                            m.y = sety;
                            matrix[starty][startx] = 0;
                            matrix[sety][setx] = m.kind;

                        }
                    }
                }
                startx = -1;
                starty = -1;
            }

            redraw();
        }
        private void p74_Click(object sender, EventArgs e)
        {
            int setx = 4;
            int sety = 7;
            human(setx, sety);
            
        }

        private void p76_Click(object sender, EventArgs e)
        {
            int setx = 6;
            int sety = 7;
            human(setx, sety);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
           // this.FormBorderStyle = FormBorderStyle.None;
           // this.WindowState = FormWindowState.Maximized;
        }
    }
}
