import axios from "axios";
import { BASE_URL } from "./Constans";

export class AuthService {
    header = {
        headers: {
            "Content-Type": "application/json",
        },
    };

    login(login_body) {
        console.log({ login_body });
        return axios.post(`${BASE_URL}/user/login`, login_body, this.header);
    }

    register(user_body) {
        console.log({ user_body });
        return axios.post(`${BASE_URL}/user/register`, user_body, this.header);
    }

    updateUser(user) {
        return axios.put(`${BASE_URL}/user/update/${user._id}`, user, this.header);
    }

    add_publication(publication_body) {
        console.log({ publication_body });
        let body = {
            text: publication_body.text,
            image: publication_body.image,
        };
        return axios.post(`${BASE_URL}/posts/${publication_body.username}/create`, body, this.header);
    }

    get_publications(username) {
        console.log(username);
        return axios.get(`${BASE_URL}/posts/${username}`, {}, this.header);
    }

    get_notFriendsUsers(id) {
        console.log({ id });
        return axios.get(`${BASE_URL}/notFriends/${id}`, {}, this.header);
    }

    send_FriendshipRequest(request_body) {
        console.log({ request_body });
        return axios.post(`${BASE_URL}/requests/send`, request_body, this.header);
    }

    get_Requests(username) {
        return axios.get(`${BASE_URL}/requests/${username}`, this.header);
    }

    accept_request(idUser, idRequest) {
        return axios.post(`${BASE_URL}/requests/${idUser}/accept/${idRequest}`, {}, this.header);
    }

    reject_request(idRequest) {
        return axios.post(`${BASE_URL}/requests/${idRequest}/reject`, {}, this.header);
    }

    get_profile(id) {
        return axios.get(`${BASE_URL}/profile/${id}`, {}, this.header);
    }
}
