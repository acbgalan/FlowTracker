export interface UserRegisterRequestInterface {
    email: string;
    firstName: string;
    password: string;
    lastName?: string | null;
}
