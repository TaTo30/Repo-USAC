#include "Estructuras/LinkedList.h"
#include "Estructuras/CircleLinkedList.h"
#include "Estructuras/Queue.h"
#include "Estructuras/BinaryTree.h"
#include "Estructuras/Matrix.h"
#include "Estructuras/OrderListP.h"
#include "Clases/Jugador.h"
#include "Paquetes/json.hpp"
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <windows.h>
#include <iostream>
#include <conio.h>
#include <stdlib.h>
#include <fstream>
#define ENTER 13
#define ESC 27



using namespace std;
using json = nlohmann::json;

//VARIABLES QUE SE DEFINEN CON ARCHIVO DE CONFIGUARCION
LinkedList<string>* Diccionario = new LinkedList<string>();
Matrix<Ficha>* PalabraTemporal = new Matrix<Ficha>();
Matrix<Ficha>* CasillasDobles = new Matrix<Ficha>();
Matrix<Ficha>* CasillasTriples = new Matrix<Ficha>();  
Matrix<Ficha>* Tablero = new Matrix<Ficha>();
BinaryTree<Jugador>* Jugadores = new BinaryTree<Jugador>();
LinkedListP<Jugador>* ListaJugadores = new LinkedListP<Jugador>();
OrderList<string>* MejoresPunteos = new OrderList<string>();
Jugador* Player1;
Jugador* Player2;
int PuntosP1=0;
int PuntosP2=0;
//VARIABLES YA DEFINIDAS DEL JUEGO
Queue<Ficha>* Fichas = new Queue<Ficha>();
int Dimension=0;
int PostOrdenVisitado = 0, InOrdenVisitado = 0, preOrdenVisitado = 0;
int POSX=0, POSY=0;
//DEFINICION DE METODOS
void EstablecerColaFichas();
void MenuPrincipal();
void PintarMarco();
void Gotoxy(int x, int y);
void ModoAbrirArchivoJson();
void ModoAgregarUsuario();
void MenuReportes();
string ArbolJugadores(BinaryTree<Jugador>::Nodo<Jugador>* raiz);
string PreOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz);
string InOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz);
string PostOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz);
void MenuSelectP1();
void MenuSelectP2();
void Turno(Jugador* P);
void ModoSeleccion(Jugador* P);
void ImprimirTablero();
void ModoScoreBoard();

//DESARROLLO DE METODOS
void ModoScoreBoard(){
    int n;
    Gotoxy(5,2);cout<<"SCOREBOARD";
    Gotoxy(7,3);cout<<"*Seleccione al Jugador 1";
    Gotoxy(7,5);cout<<"0> Mejores Punteos";
    for (int i = 0; i < ListaJugadores->Size(); i++)
    {
        Gotoxy(7,6+i);cout<<i+1<<"> "<<ListaJugadores->Index(i)->dato->GetNombre();
    }
    Gotoxy(7,6+ListaJugadores->Size());cout<<"SELECT> ";cin>>n;
    if (n >= 0 && n < ListaJugadores->Size()+1)
    {
        if (n == 0)
        {
            string graph = "digraph G {rankdir = LR; Node [shape=\"box\"];\n";
            //CREAR NODOS
            for (int i = 0; i < MejoresPunteos->Size(); i++)
            {
                graph += "N"+to_string(i)+" [label = \""+MejoresPunteos->Index(i)->dato+", "+to_string(MejoresPunteos->Index(i)->orderIndex)+"\"];\n";
            }
            //LINKEAR NODOS
            for (int i = 0; i < MejoresPunteos->Size(); i++)
            {
                if (MejoresPunteos->Index(i)!=MejoresPunteos->Max())
                {
                    graph += "N"+to_string(i)+" -> N"+to_string(i+1)+";\n";
                }                
            }
            graph +="}";
            ofstream file;
            file.open("Dot\\MejoresPlayers.dot");
            file<<graph;
            file.close();
            system("dot -Tpng Dot\\MejoresPlayers.dot -o PNG\\MejoresPlayers.png");
            system("PNG\\MejoresPlayers.png");
            system("cls");
            PintarMarco();
            ModoScoreBoard(); 
            
        }else{
            Jugador* temp = ListaJugadores->Index(n-1)->dato;
            string graph = "digraph G {rankdir = LR; Node [shape=\"box\"];\n";
            OrderList<int>* listaTemp = temp->GetPuntajesPlayer();
            graph += listaTemp->Index(0)->dato+";\n";
            for (int i = 0; i < listaTemp->Size(); i++)
            {
                if (listaTemp->Index(i)!=listaTemp->Max())
                {
                    graph += listaTemp->Index(i)->dato +"->"+to_string(listaTemp->Index(i+1)->dato)+";\n";
                }                
            }
            graph +="}";
            ofstream file;
            file.open("Dot\\Player.dot");
            file<<graph;
            file.close();
            system("dot -Tpng Dot\\Player.dot -o PNG\\Player.png");
            system("PNG\\Player.png");
            system("cls");
            PintarMarco();
            ModoScoreBoard();               
        }
        
    }else{
        Gotoxy(7,7+ListaJugadores->Size());cout<<"Seleccion no Valida";
        system("cls");
        PintarMarco();
        ModoScoreBoard();
    }
}

void ImprimirTablero(){
    string graph = "digraph G { node [shape = box]; MT [label=\"Tablero\"];";
    //INSERTAMOS TODAS LAS X
    for (int i = 0; i < Tablero->Size(); i++)
    {
        int subX = Tablero->Index(i)->X;
        graph += "X" + to_string(subX) + " [label= \""+to_string(subX)+"\" ]\n";
    }
    //ANIDAMOS AL MISMO RANGO DE ESTADO
    graph += "{rank = same;MT";
    for (int i = 0; i < Tablero->Size(); i++)
    {
        int subX = Tablero->Index(i)->X;
        graph += ";X"+to_string(subX);
    }
    graph += "}\n";

    //INSERTAMOS TODAS LAS Y
    for (int i = 0; i < Tablero->Size(); i++)
    {
        int subY = Tablero->Index(i)->Y;
        graph += "Y" + to_string(subY) + " [label= \""+to_string(subY)+"\" ]\n";
    }

    //INSERTAMOD TODOS LOS NODOS
    for (int i = 0; i < Tablero->Size(); i++)
    {

        graph += "N" + to_string(i)+ " [label= \""+Tablero->Index(i)->Dato->GetLetra()+"\" ]\n";
        graph += "{rank = same; Y"+to_string(Tablero->Index(i)->Y)+"; N"+to_string(i)+"}\n";
        graph += "Y"+to_string(Tablero->Index(i)->Y)+" -> N"+to_string(i)+";";
        graph += "X"+to_string(Tablero->Index(i)->X)+" -> N"+to_string(i)+";";
    }
    
    //LINKEAMOS TODAS LAS X
    for (int i = 0; i < Tablero->Size(); i++)
    {
        if (Tablero->Index(i)->Siguiente != NULL)
        {
            graph += "X"+to_string(Tablero->Index(i)->X)+" -> X"+to_string(Tablero->Index(i+1)->X)+";\n";
        }        
    }
    //LINKEAMOS TODAS LAS Y
    for (int i = 0; i < Tablero->Size(); i++)
    {
        if (Tablero->Index(i)->Siguiente != NULL)
        {
            graph += "Y"+to_string(Tablero->Index(i)->Y)+" -> Y"+to_string(Tablero->Index(i+1)->Y)+";\n";
        }        
    }
    //LINKEAMOS LA RAIZ CON X Y Y
    graph += "MT -> X"+to_string(Tablero->Index(0)->X)+";\n";
    graph += "MT -> Y"+to_string(Tablero->Index(0)->Y)+";\n";
    graph += "}";
    ofstream file; 
    file.open("Dot\\Tablero.dot");
    file<<graph;
    file.close();
    system("dot -Tpng Dot\\Tablero.dot -o PNG\\Tablero.png");
    system("PNG\\Tablero.png");
    
}

void MenuSelectP1(){
    int n;
    Gotoxy(5,2);cout<<"INICIAR PARTIDA SCRABBLE";
    Gotoxy(7,3);cout<<"*Seleccione al Jugador 1";
    for (int i = 0; i < ListaJugadores->Size(); i++)
    {
        Gotoxy(7,4+i);cout<<i<<"> "<<ListaJugadores->Index(i)->dato->GetNombre();
    }
    Gotoxy(7,5+ListaJugadores->Size());cout<<"SELECT> ";cin>>n;
    if (n >= 0 && n < ListaJugadores->Size())
    {
        Player1 = ListaJugadores->Index(n)->dato;
        for (int i = 0; i < 7; i++)
        {
            Player1->GetFichasPlayer()->AddLast(Fichas->Dequeue()->dato);
        }        
        system("cls");
        PintarMarco();
        MenuSelectP2();
    }else{
        Gotoxy(7,6+ListaJugadores->Size());cout<<"Seleccion no Valida";
        system("cls");
        PintarMarco();
        MenuSelectP1();
    }
}

void MenuSelectP2(){
    int n;
    int n2;
    Gotoxy(5,2);cout<<"INICIAR PARTIDA SCRABBLE";
    Gotoxy(7,3);cout<<"*Seleccione al Jugador 2";
    for (int i = 0; i < ListaJugadores->Size(); i++)
    {
        if (ListaJugadores->Index(i)->dato != Player1)
        {
            Gotoxy(7,4+i);cout<<i<<"> "<<ListaJugadores->Index(i)->dato->GetNombre();
        }else{
            n2=i;
        }
        
    }
    Gotoxy(7,5+ListaJugadores->Size());cout<<"SELECT> ";cin>>n;
    if (n >= 0 && n < ListaJugadores->Size() &&  n != n2)
    {
        Player2 = ListaJugadores->Index(n)->dato;
        for (int i = 0; i < 7; i++)
        {
            Player2->GetFichasPlayer()->AddLast(Fichas->Dequeue()->dato);
        }
        system("cls");
        PintarMarco();
        int a = rand() % 2;
        if (a == 0)
        {
            Turno(Player1);
        }else{
            Turno(Player2);
        }
        
    }else{
        Gotoxy(7,6+ListaJugadores->Size());cout<<"Seleccion no Valida";
        system("cls");
        PintarMarco();
        MenuSelectP2();
    }
}

void Turno(Jugador* P){
    Gotoxy(5,2);cout<<"Casillas Triples (Posicion)";
    Gotoxy(5,3);
    for (int i = 0; i < CasillasTriples->Size(); i++)
    {
        cout<<"X:"<<CasillasTriples->Index(i)->X<<" Y:"<<CasillasTriples->Index(i)->Y<<" - ";
    }
    Gotoxy(5,5);cout<<"Casillas Dobles (Posicion)";
    Gotoxy(5,6);
    for (int i = 0; i < CasillasDobles->Size(); i++)
    {
        cout<<"X:"<<CasillasDobles->Index(i)->X<<" Y:"<<CasillasDobles->Index(i)->Y<<" - ";
    }
    Gotoxy(5,8);cout<<"Casillas Ocupadas";
    Gotoxy(5,9);
    for (int i = 0; i < Tablero->Size(); i++)
    {
        cout<<"X:"<<Tablero->Index(i)->X<<" Y:"<<Tablero->Index(i)->Y<<" - ";
    }
    Gotoxy(5,12);cout<<"Dimension del Tablero"<<Dimension;
    Gotoxy(5,13);cout<<"Fichas de "<<P->GetNombre();
    Gotoxy(5,14);
    for (int i = 0; i < P->GetFichasPlayer()->Size(); i++)
    {
        cout<<P->GetFichasPlayer()->Index(i)->dato->GetLetra()<<" ("<<i<<") - ";
    }
    Gotoxy(5,16);cout<<"Diccionario";
    Gotoxy(5,17);
    for (int i = 0; i < Diccionario->Size(); i++)
    {
        cout<<Diccionario->Index(i)->dato<<" - ";
    }
    Gotoxy(5,20);cout<<"PALABRA A FORMAR: ";
    for (int i = 0; i < PalabraTemporal->Size(); i++)
    {
        cout<<PalabraTemporal->Index(i)->Dato->GetLetra()<<"("<<PalabraTemporal->Index(i)->X<<","<<PalabraTemporal->Index(i)->Y<<"), ";
    }
    
    char a;
    Gotoxy(5,22);cout<<"Pulse V para validar su Turno o E para Terminar la partida";
    Gotoxy(5,23);cout<<"SELECCIONAR FICHA> ";cin>>a;
    //VALIDAR LA PARTIDA
    if (a == 'V')
    {
        bool Xconstante = false;
        if (PalabraTemporal->Index(0)->X == PalabraTemporal->Index(1)->X)
        {
            Xconstante = true;
        }
        bool restriccion=true;
        if (Xconstante)
        {
            int xt = PalabraTemporal->Index(0)->X;
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                if (PalabraTemporal->Index(i)->X != xt)
                {
                    restriccion = false;
                }               
            }            
        }else{
            int yt = PalabraTemporal->Index(0)->Y;
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                if (PalabraTemporal->Index(i)->Y != yt)
                {
                    restriccion = false;
                } 
            }            
        }
        
        //REORDENAR LA PALABRA PONIENDOLA DENTRO DE UN ORDERLIST
        OrderListP<Ficha>* PalabraOrdenada = new OrderListP<Ficha>();
        if (Xconstante)
        {
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                PalabraOrdenada->Add(PalabraTemporal->Index(i)->Dato,PalabraTemporal->Index(i)->Y);
            }            
        }else{
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                PalabraOrdenada->Add(PalabraTemporal->Index(i)->Dato,PalabraTemporal->Index(i)->X);
            } 
        }
        

        string palabraFormada="";
        bool Valido=false;
        for (int i = 0; i < PalabraTemporal->Size(); i++)
        {
            palabraFormada += PalabraOrdenada->Index(i)->dato->GetLetra();
        }
        for (int i = 0; i < Diccionario->Size(); i++)
        {
            if (Diccionario->Index(i)->dato == palabraFormada)
            {
                Valido = true;
            }
        }
        //SI ES VALIDO SUMAR LOS PUNTOS AGREGARLOS AL PUNTEO
        //-VACIAR LA LISTA TEMPORAL
        //INICIAR EL NUEVO TURNO
        if (Valido && restriccion)
        {
            Gotoxy(5,18);cout<<"Tu Palabra es Valida";
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                if (CasillasTriples->Position(PalabraTemporal->Index(i)->X,PalabraTemporal->Index(i)->Y))
                {
                    //SI ESTA FICHA ESTA EN UNA POSICION TRIPLE ENTONCES SE MULTIPLICA
                    if (P == Player1)
                    {
                        PuntosP1 += 3*(PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }else{
                        PuntosP2 += 3*(PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }
                }else if (CasillasDobles->Position(PalabraTemporal->Index(i)->X,PalabraTemporal->Index(i)->Y))
                {
                    //SI ESTA FICHA ESTA EN UNA POSICION DOBLE
                    if (P == Player1)
                    {
                        PuntosP1 += 2*(PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }else{
                        PuntosP2 += 2*(PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }
                }else{
                    if (P == Player1)
                    {
                        PuntosP1 += (PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }else{
                        PuntosP2 += (PalabraTemporal->Index(i)->Dato->GetPuntaje());
                    }
                }
            }
            char tecla =' ';
            while (tecla != ENTER)
            {
                if (kbhit())
                {
                    tecla = getch();
                }       
            }
            system("cls");
            PintarMarco();
            if (P == Player1)
            {
                PalabraTemporal->Clear();
                Turno(Player2);
            }else{
                PalabraTemporal->Clear();
                Turno(Player1);
            }
        }
        else
        {
            //SI NO ES VALIDO DEVOLVER FICHAS A LA COLA
            //ELIMINAR DEL TABLERO
            //-VACIAR LA LISTA
            //INICIAR EL NUEVO TURNO
            Gotoxy(5,18);cout<<"Tu Palabra no es Valida";
            for (int i = 0; i < PalabraTemporal->Size(); i++)
            {
                Fichas->Enqueue(PalabraTemporal->Index(i)->Dato);
                Tablero->Remove(Tablero->Find(PalabraTemporal->Index(i)->Dato));
            }
            char tecla =' ';
            while (tecla != ENTER)
            {
                if (kbhit())
                {
                    tecla = getch();
                }       
            }
            system("cls");
            PintarMarco();
            if (P == Player1)
            {
                PalabraTemporal->Clear();
                Turno(Player2);
            }else{
                PalabraTemporal->Clear();
                Turno(Player1);
            }
        }
    }
    //TERMINAR LA PARTIDA
    else if (a == 'E')
    {
        Player1->GetFichasPlayer()->Clear();
        Player1->GetPuntajesPlayer()->Add(PuntosP1,PuntosP1);
        MejoresPunteos->Add(Player1->GetNombre(),PuntosP1);
        PuntosP1=0;
        Player2->GetFichasPlayer()->Clear();
        Player2->GetPuntajesPlayer()->Add(PuntosP2,PuntosP2);
        MejoresPunteos->Add(Player2->GetNombre(),PuntosP2);
        PuntosP2=0;
        system("cls");
        MenuPrincipal();
    }
    //AÑADIR PALABRA AL DICCIONARIO
    else
    {
        char b;
        Gotoxy(5,25);cout<<"<1> Agregar Esta Ficha a la Palabra  <2> Intercambiar esta Ficha con la Cola";
        Gotoxy(5,26);cout<<"SELECT> ";cin>>b;
        //AGREGAR PALABRA AL PALABRA A FORMAR
        if (b == '1')
        {
            int posx;
            int posy;
            Gotoxy(5,27);cout<<"POS X> ";cin>>posx;
            Gotoxy(5,28);cout<<"POS Y> ";cin>>posy;
            if (!Tablero->Position(posx,posy))
            {
                //SI LA POSICION NO ESTA OPCUPADA
                if (!PalabraTemporal->Contain())
                {
                    //SI LA LISTA NO TIENE DATOS
                    POSX = posx;
                    POSY = posy;
                    //AÑADIMOS LA FICHA AL TABLERO
                    Tablero->Add(P->GetFichasPlayer()->Index((int)a-48)->dato,POSX,POSY);
                    //VERIFICAMOS SI SE JUNTA CON OTRA PALABRA VERIFICANDO 4 LADOS
                    //Arriba
                    if (Tablero->Position(POSX,POSY-1))
                    {
                        PalabraTemporal->Add(Tablero->GetPosition(POSX, POSY-1)->Dato,POSX, POSY-1);
                    }//Abajo
                    else if (Tablero->Position(POSX, POSY+1))
                    {
                        PalabraTemporal->Add(Tablero->GetPosition(POSX, POSY+1)->Dato,POSX, POSY+1);
                    }//Izquierda
                    else if (Tablero->Position(POSX-1, POSY))
                    {
                        PalabraTemporal->Add(Tablero->GetPosition(POSX-1,POSY)->Dato,POSX-1,POSY);
                    }//Derecha
                    else if (Tablero->Position(POSX+1,POSY))
                    {
                        PalabraTemporal->Add(Tablero->GetPosition(POSX+1,POSY)->Dato,POSX+1,POSY);
                    }                   
                    PalabraTemporal->Add(P->GetFichasPlayer()->Index((int)a-48)->dato,POSX,POSY);
                    P->GetFichasPlayer()->RemoveAt((int)a-48);
                    P->GetFichasPlayer()->AddLast(Fichas->Dequeue()->dato);
                    ImprimirTablero();
                    char tecla =' ';
                    while (tecla != ENTER)
                    {
                        if (kbhit())
                        {
                            tecla = getch();
                        }       
                    }
                    system("cls");
                    PintarMarco();
                    Turno(P);
                }
                else
                {
                    //SI LA LISTA TIENE DATOS
                    if (((posx-POSX==1) && (posy-POSY==0)) || ((posx-POSX==0) && (posy-POSY==1)))
                    {
                        //SI ES ADYACENTE A LA ULTIMA FICHA
                        POSX = posx;
                        POSY = posy;
                        Tablero->Add(P->GetFichasPlayer()->Index((int)a-48)->dato,POSX,POSY);
                        PalabraTemporal->Add(P->GetFichasPlayer()->Index((int)a-48)->dato,POSX,POSY);
                        P->GetFichasPlayer()->RemoveAt((int)a-48);
                        P->GetFichasPlayer()->AddLast(Fichas->Dequeue()->dato);
                        ImprimirTablero();
                        char tecla =' ';
                        while (tecla != ENTER)
                        {
                            if (kbhit())
                            {
                                tecla = getch();
                            }       
                        }
                        system("cls");
                        PintarMarco();
                        Turno(P);
                    }else{
                        Gotoxy(5,20);cout<<"Las posicion no es Adyacente a una Ficha";
                        char tecla =' ';
                        while (tecla != ENTER)
                        {
                            if (kbhit())
                            {
                                tecla = getch();
                            }       
                        }
                        system("cls");
                        PintarMarco();
                        Turno(P);
                    }
                }
            }
        }
        else if (b == '2')
        {
            Fichas->Enqueue(P->GetFichasPlayer()->Index((int)a-48)->dato);
            P->GetFichasPlayer()->RemoveAt((int)a-48);
            P->GetFichasPlayer()->AddLast(Fichas->Dequeue()->dato);
            Gotoxy(5,18);cout<<"Tu nueva FICHA es una "<<P->GetFichasPlayer()->GetLast()->dato->GetLetra();
            
            //CUANDO SE CAMBIO FICHA SE TERMINA EL TURNO
            char tecla =' ';
            while (tecla != ENTER)
            {
                if (kbhit())
                {
                    tecla = getch();
                }       
            }
            system("cls");
            PintarMarco();
            if (P == Player1)
            {
                PalabraTemporal->Clear();
                Turno(Player2);
            }else{
                PalabraTemporal->Clear();
                Turno(Player1);
            }            
        }
    } 
}

void EstablecerColaFichas(){
    int c;
    LinkedList<int>* numerosSeleccionados = new LinkedList<int>();
    srand(time(NULL));
    for (int i = 0; i < 95; i++)
    {
        c = 1+rand() % (96-1);        
        if (numerosSeleccionados->Find(c) == NULL)
        {
            numerosSeleccionados->AddLast(c);
            if (c >= 1 && c <= 12)
            {
                Ficha* aux = new Ficha('A',1);                
                Fichas->Enqueue(aux);
            }
            else if (c >= 13 && c <= 24)
            {
                Ficha* aux = new Ficha('E',1);
                Fichas->Enqueue(aux);                
            }           
            else if (c >= 25 && c <= 33)
            {
                Ficha* aux = new Ficha('O',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 34 && c <= 39)
            {
                Ficha* aux = new Ficha('I',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 40 && c <= 45)
            {
                Ficha* aux = new Ficha('S',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 46 && c <= 50)
            {
                Ficha* aux = new Ficha('N',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 51 && c <= 54)
            {
                Ficha* aux = new Ficha('L',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 55 && c <= 59)
            {
                Ficha* aux = new Ficha('R',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 60 && c <= 64)
            {
                Ficha* aux = new Ficha('U',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 65 && c <= 68)
            {
                Ficha* aux = new Ficha('T',1);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 69 && c <= 73)
            {
                Ficha* aux = new Ficha('D',2);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 74 && c <= 75)
            {
                Ficha* aux = new Ficha('G',2);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 76 && c <= 79)
            {
                Ficha* aux = new Ficha('C',3);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 80 && c <= 81)
            {
                Ficha* aux = new Ficha('B',3);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 82 && c <= 83)
            {
                Ficha* aux = new Ficha('M',3);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 84 && c <= 85)
            {
                Ficha* aux = new Ficha('P',3);
                Fichas->Enqueue(aux);                
            }
            else if (c >= 86 && c <= 87)
            {
                Ficha* aux = new Ficha('H',4);
                Fichas->Enqueue(aux);                
            }
            else if (c == 88)
            {
                Ficha* aux = new Ficha('F',4);
                Fichas->Enqueue(aux);                
            }
            else if (c == 89)
            {
                Ficha* aux = new Ficha('V',4);
                Fichas->Enqueue(aux);                
            }
            else if (c == 90)
            {
                Ficha* aux = new Ficha('Y',4);
                Fichas->Enqueue(aux);                
            }
            else if (c == 91)
            {
                Ficha* aux = new Ficha('Q',5);
                Fichas->Enqueue(aux);                
            }
            else if (c == 92)
            {
                Ficha* aux = new Ficha('J',8);
                Fichas->Enqueue(aux);                
            }
            else if (c == 93)
            {
                Ficha* aux = new Ficha((char)165,8);
                Fichas->Enqueue(aux);                
            }
            else if (c == 94)
            {
                Ficha* aux = new Ficha('X',8);
                Fichas->Enqueue(aux);                
            }
            else if (c == 95)
            {
                Ficha* aux = new Ficha('Z',10);
                Fichas->Enqueue(aux);                
            }
        }else{
            i--;
        }       
    }    
}

void ModoAgregarUsuario(){
    string username;
    Gotoxy(5,1);cout<<"[Nombre de Usuario]: ";cin>>username;
    if (!Jugadores->Find((int)username[0]))
    {
        Jugador* aux = new Jugador(username);
        Jugadores->Add(aux,(int)username[0]);
        ListaJugadores->AddLast(aux);
        Gotoxy(5,2);cout<<(char)173<<(char)173<<"Usuario Agregado con Exito!!";
        char tecla =' ';
        while (tecla != ENTER)
        {
            if (kbhit())
            {
                tecla = getch();
            }       
        }
        system("cls");
        MenuPrincipal();
    }
    else
    {
        Gotoxy(5,2);cout<<(char)173<<(char)173<<"Nombre de Usuario no permitido!!";
        char tecla =' ';
        while (tecla != ENTER)
        {
            if (kbhit())
            {
                tecla = getch();
            }       
        }
        system("cls");
        PintarMarco();
        ModoAgregarUsuario();
    }
}

void MenuReportes(){
    char a;
    Gotoxy(5,2);cout<<"SELECCIONE EL REPORTE QUE DESEA VER";
    Gotoxy(5,3);cout<<"0> Regresar al menu anterior";
    Gotoxy(5,4);cout<<"1> Diccionario";
    Gotoxy(5,5);cout<<"2> Cola de Fichas";
    Gotoxy(5,6);cout<<"3> Usuarios";
    Gotoxy(5,7);cout<<"4> Usuarios <PreOrden>";
    Gotoxy(5,8);cout<<"5> Usuarios <InOrden>";
    Gotoxy(5,9);cout<<"6> Usuarios <PostOrden>";
    Gotoxy(5,10);cout<<"7> Tablero";
    Gotoxy(5,11);cout<<"SELECT> ";cin>>a;
    if (a == '0')
    {
        system("cls");
        MenuPrincipal();
    }    
    else if (a == '1')
    {
        //REPORTE DICCIONARIO
        string graph = "digraph g { rankdir=LR; node [shape=box];\n";
        for (int i = 0; i < Diccionario->Size(); i++)
        {
            if (Diccionario->Index(i) == Diccionario->GetFirst())
            {
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->Index(i+1)->dato+"\n";
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->GetLast()->dato+"\n";
            }
            else if (Diccionario->Index(i) == Diccionario->GetLast())
            {
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->GetFirst()->dato+"\n";
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->Index(i-1)->dato+"\n";
            }
            else
            {
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->Index(i+1)->dato+"\n";
                graph += Diccionario->Index(i)->dato+"->"+Diccionario->Index(i-1)->dato+"\n";
            }
        }
        graph += "}";
        ofstream o;
        o.open("Dot\\Diccionario.dot");
        o<<graph;
        o.close();
        system("dot -Tpng Dot\\Diccionario.dot -o PNG\\Diccionario.png");
        system("PNG\\Diccionario.png");
        system("cls");
        PintarMarco();
        MenuReportes();        
    }
    else if (a == '2')
    {
        ofstream file;
        file.open("Dot\\ColaFichas.dot");
        file<<"digraph g{ rankdir = LR; node [shape = record]; struct1 [label=\"";
        Queue<Ficha>* temp = Fichas->Clonar();
        int QueueSize = temp->Size();
        for (int i = 0; i < QueueSize; i++)
        {
            Ficha* aux = temp->Dequeue()->dato;
            file<<"<f"<<i<<">"<<aux->GetLetra()<<" x"<<aux->GetPuntaje()<<"|";
        }
        file<<"\"]; }";
        file.close();
        system("dot -Tpng Dot\\ColaFichas.dot -o PNG\\ColaFichas.png");
        system("PNG\\ColaFichas.png");
        system("cls");
        PintarMarco();
        MenuReportes();        
    }
    else if (a == '3')
    {
        BinaryTree<Jugador>::Nodo<Jugador>* temp = Jugadores->GetRoot();
        string graph = "digraph g{ node [shape = circle];";
        graph += temp->Dato->GetNombre()+";\n";
        graph += ArbolJugadores(temp) +"}";
        ofstream file;
        file.open("Dot\\Jugadores.dot");
        file<<graph;
        file.close();
        system("dot -Tpng Dot\\Jugadores.dot -o PNG\\Jugadores.png");
        system("PNG\\Jugadores.png");
        system("cls");
        PintarMarco();
        MenuReportes();
    }
    else if (a == '4')
    {
        BinaryTree<Jugador>::Nodo<Jugador>* temp = Jugadores->GetRoot();
        string graph = "digraph g{ rankdir = LR; node [shape = box];";
        //graph += temp->Dato->GetNombre()+";\n";
        graph += PreOrden(temp) +"}";
        ofstream file;
        file.open("Dot\\Jugadores_PreOrden.dot");
        file<<graph;
        file.close();
        preOrdenVisitado = 0;
        system("dot -Tpng Dot\\Jugadores_PreOrden.dot -o PNG\\Jugadores_PreOrden.png");
        system("PNG\\Jugadores_PreOrden.png");
        system("cls");
        PintarMarco();
        MenuReportes();
    }
    else if (a == '5')
    {
        BinaryTree<Jugador>::Nodo<Jugador>* temp = Jugadores->GetRoot();
        string graph = "digraph g{ rankdir = LR; node [shape = box];";
        //graph += temp->Dato->GetNombre()+";\n";
        graph += InOrden(temp) +"}";
        ofstream file;
        file.open("Dot\\Jugadores_InOrden.dot");
        file<<graph;
        file.close();
        InOrdenVisitado = 0;
        system("dot -Tpng Dot\\Jugadores_InOrden.dot -o PNG\\Jugadores_InOrden.png");
        system("PNG\\Jugadores_InOrden.png");
        system("cls");
        PintarMarco();
        MenuReportes();
    }
    else if (a == '6')
    {
        BinaryTree<Jugador>::Nodo<Jugador>* temp = Jugadores->GetRoot();
        string graph = "digraph g{ rankdir = LR; node [shape = box];";
        //graph += temp->Dato->GetNombre()+";\n";
        graph += PostOrden(temp) +"}";
        ofstream file;
        file.open("Dot\\Jugadores_PostOrden.dot");
        file<<graph;
        file.close();
        PostOrdenVisitado = 0;
        system("dot -Tpng Dot\\Jugadores_PostOrden.dot -o PNG\\Jugadores_PostOrden.png");
        system("PNG\\Jugadores_PostOrden.png");
        system("cls");
        PintarMarco();
        MenuReportes();
    }
    else if (a == '7')
    {
        ImprimirTablero();
    }
}

string ArbolJugadores(BinaryTree<Jugador>::Nodo<Jugador>* raiz){
    if (raiz != NULL)
    {
        string temp="";
        if(raiz->HijoIzquierdo != NULL)
        {
            temp += raiz->Dato->GetNombre()+"->"+raiz->HijoIzquierdo->Dato->GetNombre()+";\n";
        } 
        if (raiz->HijoDerecho != NULL)
        {
            temp += raiz->Dato->GetNombre()+"->"+raiz->HijoDerecho->Dato->GetNombre()+";\n";
        }
               
        temp += ArbolJugadores(raiz->HijoIzquierdo);
        temp += ArbolJugadores(raiz->HijoDerecho);
        return temp;
    }
    else
    {
        return "";
    }    
}

string PreOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz){
    if (raiz != NULL)
    {
        string temp="";
        if ((preOrdenVisitado+1) == Jugadores->Size())
        {
            temp += raiz->Dato->GetNombre();   
        }
        else
        {
            temp += raiz->Dato->GetNombre()+"->";
            preOrdenVisitado++;
        }
        temp += PreOrden(raiz->HijoIzquierdo);
        temp += PreOrden(raiz->HijoDerecho);    
        return temp;    
    }
    else
    {
        return "";
    }
}

string InOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz){
    if (raiz != NULL)
    {
        string temp="";        
        temp += InOrden(raiz->HijoIzquierdo);
        if ((InOrdenVisitado+1) == Jugadores->Size())
        {
            temp += raiz->Dato->GetNombre();   
        }
        else
        {
            temp += raiz->Dato->GetNombre()+"->";
            InOrdenVisitado++;
        }
        temp += InOrden(raiz->HijoDerecho);    
        return temp;    
    }
    else
    {
        return "";
    }
}

string PostOrden(BinaryTree<Jugador>::Nodo<Jugador>* raiz){
    if (raiz != NULL)
    {
        string temp="";        
        temp += PostOrden(raiz->HijoIzquierdo);        
        temp += PostOrden(raiz->HijoDerecho);  
        if ((PostOrdenVisitado+1) == Jugadores->Size())
        {
            temp += raiz->Dato->GetNombre();   
        }
        else
        {
            temp += raiz->Dato->GetNombre()+"->";
            PostOrdenVisitado++;
        }  
        return temp;    
    }
    else
    {
        return "";
    }
}

void MenuPrincipal(){
    char a;
	PintarMarco();
    Gotoxy(5,3);cout<<"UNIVERSIDAD DE SAN CARLOS DE GUATEMALA";
	Gotoxy(5,4);cout<<"FACULTAD DE INGENIERIA";
	Gotoxy(5,5);cout<<"ESTRUCTURAS DE DATOS";
	Gotoxy(5,6);cout<<"PROYECTO 1";
	Gotoxy(5,7);cout<<"ALDO RIGOBERTO HERNANDEZ AVILA";
	Gotoxy(5,8);cout<<"201800585";
    Gotoxy(5,10);cout<<"MEN"<<(char)233;
	Gotoxy(5,11);cout<<"1> Archivo de Configuracion";
	Gotoxy(5,12);cout<<"2> Jugar";
    Gotoxy(5,13);cout<<"3> A"<<(char)164<<"adir Usuario";
	Gotoxy(5,14);cout<<"4> Reportes";
    Gotoxy(5,15);cout<<"5> ScoreBoard";
	Gotoxy(5,16);cout<<"6> Salir"<<endl;
	Gotoxy(5,18);cout<<"P1> ";cin >> a;
    if (a == '1')
    {		
        ModoAbrirArchivoJson();		  
	}
	else if (a == '2')
	{
		if (Jugadores->Size() >= 2)
        {
            system("cls");
            PintarMarco();
            MenuSelectP1();
        }else{
            Gotoxy(5,19);cout<<"No hay suficientes jugadores para iniciar el juego";
            char tecla =' ';
            while (tecla != ENTER)
            {
                if (kbhit())
                {
                    tecla = getch();
                }
            }
        system("cls");
        MenuPrincipal();
        }
        
	}
	else if (a == '3')
	{	
		system("cls");
        PintarMarco();
		ModoAgregarUsuario();
	}
	else if(a == '4')
	{
        system("cls");
        PintarMarco();
		MenuReportes();
	}
    else if(a == '5')
    {
        system("cls");
        PintarMarco();
        ModoScoreBoard();  
    }
    else if(a == '6')
    {
        exit(0);
    }
}

void PintarMarco(){
	Gotoxy(3, 2);cout<<(char)201;
	for (int i = 0; i < 25; i++)
	{
		Gotoxy(3, 3+i);cout<<(char)186;
	}	
}

void Gotoxy(int x,int y){
	HANDLE hCon;
	hCon=GetStdHandle(STD_OUTPUT_HANDLE);	
	COORD dwPos;
	dwPos.X=x;
	dwPos.Y=y;	
	SetConsoleCursorPosition(hCon,dwPos);
}

void ModoAbrirArchivoJson(){
    
    string path;
    Gotoxy(5,17);cout<<"[Directorio]: ";cin>>path;
    //cout<<path;
    ifstream ArchivoConfiguracion(path);
    json config;
    json JsonDiccionario;
    json Casillas;
    json Triples;
    json Dobles;
    ArchivoConfiguracion >> config;
    Dimension = config["dimension"];
    for (json::iterator i = config.begin(); i != config.end(); ++i)
    {
        if (i.key() == config.find("diccionario").key())
        {
            JsonDiccionario = i.value();
        }
        else if (i.key() == config.find("casillas").key())
        {
            Casillas = i.value();
            for (json::iterator j = Casillas.begin(); j != Casillas.end(); ++j)
            {
                if (j.key() == Casillas.find("dobles").key())
                {
                    Dobles = j.value();
                }
                else if (j.key() == Casillas.find("triples").key())
                {
                    Triples = j.value();
                }
            }
        }
    }
    for (size_t i = 0; i < JsonDiccionario.size(); i++)
    {
        json temp = JsonDiccionario[i].get<json>();
        Diccionario->AddLast(temp["palabra"]);
        
    }
    for (size_t i = 0; i < Triples.size(); i++)
    {
        json temp = Triples[i].get<json>();
        Ficha *aux = new Ficha('#',3);
        CasillasTriples->Add(aux,temp["x"],temp["y"]);   
    }
    for (size_t i = 0; i < Dobles.size(); i++)
    {
        json temp = Dobles[i].get<json>();
        Ficha *aux = new Ficha('#',2);
        CasillasDobles->Add(aux,temp["x"],temp["y"]);        
    }  

    Gotoxy(5,20);cout<<"Terminada la lectura de archivo...";
    Gotoxy(5,21);cout<<"Pulse ENTER para regresar al menu anterior";
    char tecla =' ';
    while (tecla != ENTER)
    {
        if (kbhit())
        {
            tecla = getch();
        }
                
    }
    system("cls");
    MenuPrincipal();
    
}


//METODO PRINCIPAL
int main()
{   
    EstablecerColaFichas();
    MenuPrincipal();
    cin.get();
    cin.ignore();
       
}
