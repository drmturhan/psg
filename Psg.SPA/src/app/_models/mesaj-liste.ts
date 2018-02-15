
export class MesajListe {
    mesajId: number;
    gonderenNo?: number;
    gonderenTamAdi?: string;
    gonderenProfilFotoUrl?: string;
    alanNo?: number;
    alanTamAdi?: string;
    alanProfilFotoUrl?: string;
    icerik?: string;
    okundu?: boolean;
    okunmaZamani?: boolean;
    gonderilmeZamani?: Date;
}

export class MesajYaratma {
    gonderenNo?: number;
    alanNo?: number;
    icerik?: string;
}