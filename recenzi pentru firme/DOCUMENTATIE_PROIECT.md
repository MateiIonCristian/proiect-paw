# Documentație Proiect - FirmeHub (Recenzii Firme)

## 1. Descrierea Aplicației
FirmeHub este o platformă web modernă destinată centralizării și evaluării firmelor din diverse domenii de activitate. Utilizatorii pot găsi locații, citi recenzii și vizualiza serviciile oferite de companiile locale.

## 2. Specificații Tehnice
*   **Framework:** ASP.NET Core 10.0 (MVC Architecture)
*   **Baza de date:** SQL Server (Entity Framework Core - Code First)
*   **Frontend:** Bootstrap 5, FontAwesome, jQuery (AJAX)
*   **Arhitectură:** Controller -> Service -> Repository

## 3. Structura Bazei de Date (6 Tabele)
1.  **Firma:** Tabelul principal (Nume, Descriere, Adresă).
2.  **Categorie:** Domeniul de activitate (IT, Horeca etc.).
3.  **Oras:** Locația sediului central.
4.  **Recenzie:** Evaluările utilizatorilor (Autor, Notă, Comentariu).
5.  **Serviciu:** Listă de servicii specifice oferite de firmă.
6.  **Contact:** Detalii de contact (Email, Telefon) - Relație 1:1.

## 4. Funcționalități Implementate
*   **CRUD Complet:** Gestionare Firme, Categorii și Recenzii.
*   **Căutare AJAX:** Filtrarea firmelor în timp real fără reîncărcarea paginii.
*   **Rutare Avansată:** Folosirea Attribute Routing pentru URL-uri curate.
*   **Design Responsive:** Compatibil cu dispozitive mobile și desktop.
*   **Integrare Servicii:** Toată logica de business este izolată în Service Layer.

## 5. Dezvoltări Viitoare (Planificat)
*   Sistem de autentificare real (ASP.NET Identity).
*   Sistem de upload poze pentru logo-ul firmei.
*   Hărți Google Maps pentru locații.
*   Sistem de notificări prin email pentru recenzii noi.
