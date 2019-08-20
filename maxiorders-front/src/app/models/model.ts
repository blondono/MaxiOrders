import { User } from './users/user';

export class Login {
    constructor(
        public token: string,
        public User: User
    ){}
}