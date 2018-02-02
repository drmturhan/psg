import { Kullanici } from './kullanici';

export interface AuthUser {
    tokenString: string;
    kullanici: Kullanici;
}
