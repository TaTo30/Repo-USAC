#include <windows.h>
#include <iostream>
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <fstream>
#include "ListaDoble.cpp"
#include "Pila.cpp"
#include "ListaCircularSimple.cpp"
#define ESC 27
#define ENTER 13
#define BS 8
#define DEL 127
#define ARRIBA 72
#define IZQUIERDA 75
#define ABAJO 80
#define DERECHA 77
#define CTRLZ 26
#define CTRLS 19
#define CTRLR 18
#define CTRLW 23
#define CTRLY 25
#define CTRLX 24

using namespace std;

int pos = 0;
int lin = 0;
int BRTrue = 0;

ListaCircularSimple* archivosRecientes = new ListaCircularSimple();
ListaDoble* listaCaracteres = new ListaDoble();
Pila* logCambios = new Pila();
Pila* logRevertidos = new Pila();
Nodo* ultimoAgregado;
NodoPila* ultimoPop;
ofstream saveFile;
ifstream openFile;

void PintarMarco();
void ImprimirLista();
void gotoxy();
void ControlesEditarTexto();
void BuscarReemplazar();
void MenuInicio();
void ArchivosRecientes();

int PosicionCursorX(){
	CONSOLE_SCREEN_BUFFER_INFO csbi;
	GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
	return csbi.dwCursorPosition.X;
}

int PosicionCursorY(){
	CONSOLE_SCREEN_BUFFER_INFO csbi;
	GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
	return csbi.dwCursorPosition.Y;
}

void gotoxy(int x,int y){
	HANDLE hCon;
	hCon=GetStdHandle(STD_OUTPUT_HANDLE);	
	COORD dwPos;
	dwPos.X=x;
	dwPos.Y=y;	
	SetConsoleCursorPosition(hCon,dwPos);
}

void ImprimirLista(){
	for (int i = 0; i < listaCaracteres->Size(); i++)
	{
		if (listaCaracteres->Buscar(i)->dato == '\n')
		{
			gotoxy(5, PosicionCursorY()+1);
		}else{
			cout<<listaCaracteres->Buscar(i)->dato;
		}			
	}	
}

void PintarMarco(){
	gotoxy(3, 2);cout<<(char)201;
	for (int i = 0; i < 25; i++)
	{
		gotoxy(3, 3+i);cout<<(char)186;
	}	
}

void ControlesEditorTexto(){
	gotoxy(14,0);cout<<"Ctrl+W Reemplazar";//18
	gotoxy(37,0);cout<<"Ctrl+S Guardar";//14
	gotoxy(56,0);cout<<"Ctrl+R Reportes";//15
	gotoxy(75,0);cout<<"ESC Regresar a Inicio";//21
	gotoxy(5, 2);
}

void BuscarReemplazar(string Buscar, string Reemplazar, int PosicionInicial){
	int pos = PosicionInicial, T = Buscar.length();
	bool encontrado = false;
	Nodo* aux = listaCaracteres->Buscar(PosicionInicial);
	while(aux!=NULL){
		if (aux == listaCaracteres->ObtenerFirst())
		{
			if ((aux->dato == Buscar[0]) && (listaCaracteres->Buscar(pos + T - 1)->dato == Buscar[T-1]))
			{			
				encontrado = true;
				BRTrue++;
				aux=NULL;
			}else{
				pos++;
				aux=aux->Siguiente;
			}
		}else{
			if ((aux->dato == Buscar[0]) && (aux->Anterior->dato == ' ') && (listaCaracteres->Buscar(pos + T - 1)->dato == Buscar[T-1]))
			{			
				encontrado = true;
				BRTrue++;
				aux=NULL;
			}else{
				pos++;
				aux=aux->Siguiente;
			}			
		}		
	}
	if (encontrado)
	{
		for (int i = 0; i < T; i++)
		{
			listaCaracteres->Eliminar(listaCaracteres->Buscar(pos));
		}
		for (int i = 0; i < Reemplazar.length(); i++)
		{
			if (pos == 0)
			{
				listaCaracteres->InsertarAfterAt(listaCaracteres->Buscar(pos + i), Reemplazar[i]);
			}else{
				listaCaracteres->InsertarAfterAt(listaCaracteres->Buscar(pos + i - 1), Reemplazar[i]);
			}			
		}
		if(pos==0){
			listaCaracteres->InsertarAfterAt(listaCaracteres->Buscar((int)Reemplazar.length()), ' ');
			listaCaracteres->Eliminar(listaCaracteres->ObtenerFirst());
		}
		gotoxy(5,1);cout<<"                                                    ";gotoxy(5,1);cout<<BRTrue<<" Palabras afectadas";
		BuscarReemplazar(Buscar, Reemplazar, pos + Reemplazar.length());
	}else{
		gotoxy(5,1);cout<<"                                                    ";gotoxy(5,1);cout<<BRTrue<<" Palabras afectadas";
	}	
}

void ModoEditorTexto(){
	char tecla=' ';
	pos = 0;
	lin = 0;
	BRTrue = 0;
	while(tecla != ESC){
		if (kbhit())
		{
			tecla = getch(); 		
			switch (tecla)
			{
			case CTRLW:
				if (true)
				{
					BRTrue=0;
					int X=PosicionCursorX(), Y = PosicionCursorY();
					string sentencia;
					gotoxy(5,1);cout<<"<Buscar>;<Reemplazar>: ";std::cin>>sentencia;					
					string delimiter = ";";
					size_t pos = 0;
					string token;	
					while ((pos = sentencia.find(delimiter)) != string::npos) {
    					token = sentencia.substr(0, pos);    					
    					sentencia.erase(0, pos + delimiter.length());
					}
					BuscarReemplazar(token, sentencia,0);	
					logCambios->Push(token, sentencia);
					std::cin.get();
					std::cin.ignore();
					system("cls");
					PintarMarco();
					ControlesEditorTexto();
					ImprimirLista();
					gotoxy(X, Y);			
				}
				
			break;
			case CTRLS:
				if (true)
				{
					
					int X=PosicionCursorX(), Y = PosicionCursorY();
					string path;
					Nodo* aux = listaCaracteres->ObtenerFirst();
					gotoxy(5,1);cout<<"[Directorio]: ";std::cin>>path;
					saveFile.open(path);
					while (aux != NULL)
					{
						saveFile<<aux->dato;
						aux=aux->Siguiente;
					}
					saveFile.close();	
					gotoxy(5,1);cout<<"                                             						       ";gotoxy(5,1);cout<<"Archivo guardado";				
					std::cin.get();
					std::cin.ignore();
					system("cls");
					PintarMarco();
					ControlesEditorTexto();
					ImprimirLista();
					gotoxy(X, Y);
				}
			break;
			case CTRLZ:
				if (!logCambios->Vacio())
				{
					BRTrue=0;
					int X=PosicionCursorX(), Y = PosicionCursorY();
					ultimoPop = logCambios->Pop();
					BuscarReemplazar(ultimoPop->reemplazar, ultimoPop->buscar,0);
					logRevertidos->Push(ultimoPop->buscar, ultimoPop->reemplazar);
					std::cin.get();
					std::cin.ignore();
					system("cls");
					PintarMarco();
					ControlesEditorTexto();
					ImprimirLista();
					gotoxy(X, Y);
				}
			break;
			case CTRLY:
				if (!logRevertidos->Vacio())
				{
					BRTrue=0;
					int X=PosicionCursorX(), Y = PosicionCursorY();
					ultimoPop = logRevertidos->Pop();
					BuscarReemplazar(ultimoPop->buscar, ultimoPop->reemplazar,0);
					logCambios->Push(ultimoPop->buscar, ultimoPop->reemplazar);
					std::cin.get();
					std::cin.ignore();
					system("cls");
					PintarMarco();
					ControlesEditorTexto();
					ImprimirLista();
					gotoxy(X, Y);
				}
			break;
			case CTRLR:
				if (true)
				{
					int X=PosicionCursorX(), Y=PosicionCursorY();
					gotoxy(5,1);cout<<"[Reportes]: 1. Lista, 2. Palabras Buscadas, 3. Palabras Ordenadas";
					gotoxy(X,Y);
				}
				
			break;
			case ENTER:
				if (true)
				{
					int y = PosicionCursorY();
					pos++;
					lin++;
					gotoxy(5,y+1);				
					listaCaracteres->InsertarLast('\n');
					ultimoAgregado = listaCaracteres->ObtenerLast();
				}				
			break;
			case BS:
				if (pos > 0)
				{
					int x = PosicionCursorX(), y = PosicionCursorY();											
					listaCaracteres->Eliminar(listaCaracteres->Buscar(pos-1));
					pos--;
					system("cls");
					PintarMarco();
					ControlesEditorTexto();
					ImprimirLista();
					gotoxy(x-1,y);
				}
			break;
			case IZQUIERDA:
				if (pos > 0 && PosicionCursorX() > 5)
				{
					gotoxy(PosicionCursorX()-1,PosicionCursorY());
					pos--;
				}
			break;
			case DERECHA:
				if (pos < listaCaracteres->Size())
				{				
					gotoxy(PosicionCursorX()+1,PosicionCursorY());
					pos++;
				}
			break;
			case ARRIBA:
				if (true)
				{
					int saltosEncontrados = 0, posicionActual = 0, posicionEnCarrito= 0;
					for (int i = 0; i < listaCaracteres->Size(); i++)
					{
						if (listaCaracteres->Buscar(i)->dato == '\n')
						{
							saltosEncontrados++;		
							if (saltosEncontrados == lin)
							{
								gotoxy(posicionEnCarrito+5, PosicionCursorY()-1);
								lin--;
								i = listaCaracteres->Size();
								pos = posicionActual;
							}
							posicionEnCarrito=0;
							posicionActual++;
						}
						else
						{
							posicionActual++;
							posicionEnCarrito++;
						}		
					}								
				}				
			break;
			case ABAJO:
				if (true)
				{
					int saltosEncontrados = 0, posicionActual = 0, posicionEnCarrito= 0;
					for (int i = 0; i < listaCaracteres->Size(); i++)
					{
						if (listaCaracteres->Buscar(i)->dato == '\n' || listaCaracteres->Buscar(i)==listaCaracteres->ObtenerLast())
						{								
							if (saltosEncontrados == lin+1)
							{
								gotoxy(posicionEnCarrito+5, PosicionCursorY()+1);
								lin++;
								i = listaCaracteres->Size();
								pos = posicionActual;
							}
							else
							{
								saltosEncontrados++;
							}						
							posicionEnCarrito=0;
							posicionActual++;
						}
						else
						{
							posicionActual++;
							posicionEnCarrito++;
						}		
					}
				}
			break;		
			default:
				if ((tecla<= 126 && tecla >= 32) || (tecla <= 254 && tecla>=128) || (tecla == 10))
				{
					logCambios->Vaciar();
					logRevertidos->Vaciar();
					if (pos < listaCaracteres->Size())
					{
						int x = PosicionCursorX(), y = PosicionCursorY();
						listaCaracteres->InsertarAfterAt(listaCaracteres->Buscar(pos-1),tecla);
						pos++;
						// = listaCaracteres->Buscar(pos - 1);
						// = pos;
						system("cls");
						PintarMarco();
						ControlesEditorTexto();					
						ImprimirLista();
						gotoxy(x+1, y);
					}
					else
					{
						gotoxy(PosicionCursorX(),PosicionCursorY());
						cout<<tecla;
						listaCaracteres->InsertarLast(tecla);
						// = listaCaracteres->ObtenerLast();
						//=pos;
						pos++;	
					}						
				}
			break;
			}								
		}		
	}
	listaCaracteres->Vaciar();
	system("cls");
	MenuInicio();
	
}

void ModoAbrirArchivo(){
	int Y = 0;
	string path;
	char linea[200];
	string parser;
	gotoxy(5,1);cout<<"[Directorio]: ";std::cin>>path;
	openFile.open(path);
	if(openFile.fail()){
		cout<<"Error: No se puede abrir el archivo"<<endl;
	}else{
		archivosRecientes->Insertar(path);
		while (!openFile.eof())
		{
			openFile.getline(linea, sizeof(linea));
			parser=linea;
			for (int i = 0; i < parser.length(); i++)
			{
				listaCaracteres->InsertarLast(parser[i]);
			}
			listaCaracteres->InsertarLast('\n');
			Y++;			
		}		
	}
	openFile.close();
	system("cls");
	PintarMarco();
	ControlesEditorTexto();
	ImprimirLista();
	//cout<<"Presione ENTER para continuar";
	gotoxy(5,2);
	ModoEditorTexto();
	
	
	//otoxy(5, 3);

}

void ModoArchivosRecientes(){
	NodoCircular* aux = archivosRecientes->ObtenerFirst();
	gotoxy(5,3);
	for (int i = 0; i < archivosRecientes->Size(); i++)
	{
		gotoxy(5,3+i);cout<<"Archivo "<<i+1<<": "<<aux->dato;
		aux=aux->Siguiente;
	}
	
	
	char a;
	gotoxy(5,1);cout<<"[Reportes]: 1 [Regresar]: 2 ";std::cin>>a;
	if (a == '2')
	{
		system("cls");
		PintarMarco();
		MenuInicio();
	}else{
		/*Reporte*/
	}
	
}

void MenuInicio(){
	char a;
	PintarMarco();
    gotoxy(5,3);cout<<"UNIVERSIDAD DE SAN CARLOS DE GUATEMALA";
	gotoxy(5,4);cout<<"FACULTAD DE INGENIERIA";
	gotoxy(5,5);cout<<"ESTRUCTURAS DE DATOS";
	gotoxy(5,6);cout<<"PRACTICA 1";
	gotoxy(5,7);cout<<"ALDO RIGOBERTO HERNANDEZ AVILA";
	gotoxy(5,8);cout<<"201800585";
    gotoxy(5,10);cout<<"MENÚ";
	gotoxy(5,11);cout<<"1. Crear Archivo";
	gotoxy(5,12);cout<<"2. Abrir Archivo";
	gotoxy(5,13);cout<<"3. Archivos Recientes";
	gotoxy(5,14);cout<<"4. Salir"<<endl;
	gotoxy(5,15);
    std::cin >> a;
    if (a == '1')
    {
		system("cls");
		PintarMarco();
		ControlesEditorTexto();    	
		ModoEditorTexto();  
	}
	else if (a == '2')
	{
		system("cls");
		PintarMarco();
		ModoAbrirArchivo();
	}
	else if (a == '3')
	{
		
		system("cls");
		PintarMarco();
		ModoArchivosRecientes();
	}
	else if(a == '4')
	{
		exit(0);
	} 
}

int main()
{
    MenuInicio();
}