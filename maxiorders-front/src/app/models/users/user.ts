export class User {
    constructor(
        public IdUser: number,
        public Identification: string,
        public Surname: string,
        public Email: string,
        public Password: string,
        public Role: string,
        public Photo: string,
        public DigitalSignature: string
    ){}
}