export function validasyonMesajlari() {
    return {
        kullaniciAdi: {
            required: 'Kullanıcı adı alanına bilgi girilmesi gerekli.',
            minlength: 'En az 3 karakter olmalıdır.',
            maxlength: 'En fazla 20 karakter olmalıdır',
            boslukIceremez: 'Kullanıcı adı boşluk içermez.',
            kullaniciAdiKullaniliyor: 'Kullanıcı adı kullanımda. Başka seçin lütfen.'
        },
        unvan: {
            minlength: 'En az 2 karakter olmalıdır.',
            maxlength: 'En fazla 10 karakter olmalıdır'
        },
        ad: {
            required: 'Ad alanına bilgi girilmesi gerekli.',
            minlength: 'En az 2 karakter olmalıdır.',
            maxlength: 'En fazla 50 karakter olmalıdır'
        },
        digerAd: {
            minlength: 'En az 2 karakter olmalıdır.',
            maxlength: 'En fazla 50 karakter olmalıdır'
        },
        soyad: {
            required: 'Soyad alanına bilgi girilmesi gerekli.',
            minlength: 'En az 2 karakter olmalıdır.',
            maxlength: 'En fazla 50 karakter olmalıdır'
        },
        cinsiyetNo: {
            required: 'Cinsiyet alanına bilgi girilmesi gerekli.'
        },
        dogumTarihi: {
            required: 'Doğum tarihi alanına bilgi girilmesi gerekli.'

        },
        ePosta: {
            required: 'Eposta  girmediniz',
            email: 'Geçerli bir eposta adresi girmeliniz'
        },
        telefonNumarasi: {
            required: 'Telefon numarası girmediniz',
            maxlength: 'Telefon numarası en az 2 an fazla 10 rakamdan oluşabilir'
        },
        sifre: {
            required: 'Şifre girmediniz',
            minlength: 'En az 6 karakter olmalıdır.',
            maxlength: 'En fazla 18 karakter olmalıdır'
        },
        sifreKontrol: {
            required: 'Şifre kontrol girmediniz',
        },
        sifreGrup: {
            'sifreKontrolBasarisiz': 'Şifre ile şifre kontrol aynı olmalıdır'
        }
    };
}

export function uyelikFormuEksikMesaji() { return 'Form bilgileri eksik. Bu bilgilerle üye olamazsınız'; }

