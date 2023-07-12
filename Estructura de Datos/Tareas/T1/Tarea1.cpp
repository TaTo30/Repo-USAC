// Tarea1.cpp : Este archivo contiene la función "main". La ejecución del programa comienza y termina ahí.
//

#include <iostream>
#include <malloc.h>
using namespace std;


int main()
{
	

	int M;
	int matriz[10][10];

	cout << "Ingrese el tamaño de la matriz: ";
	cin >> M;
	matriz[1][1] = matriz[M][M];
	for (int i = 0; i < M; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (i==0 || i==M-1)
			{
				matriz[i][j] = 1;
			}
			else {
				if (j==0 || j== M-1)
				{
					matriz[i][j] = 1;
				}
				else {
					matriz[i][j] = 0;
				}
			}
		}
		
	}
	for (int i = 0; i < M; i++)
	{
		for (int j = 0; j < M; j++)
		{
			cout<<matriz[i][j];
		}
		cout << endl;
	}
}

