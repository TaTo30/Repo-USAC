import Axios from "axios";
import { Button } from "primereact/button";
import { FileUpload } from "primereact/fileupload";
import { Image } from "primereact/image";
import { InputSwitch } from "primereact/inputswitch";
import { InputText } from "primereact/inputtext";
import { Messages } from "primereact/messages";
import { Toast } from "primereact/toast";
import React, { useState, useRef, useEffect } from "react";
import { AuthService } from "../service/AuthService";
import { Message } from "primereact/message";

const InicioPerfil = () => {
    const msgDesc = "Despues de seleccionar tu archivo haz click en el boton 'UPLOAD' (Para procesar la imagen antes de enviar)";

    const authService = new AuthService();
    const toast = useRef();
    const message = useRef();
    const [baseUrl, setBaseUrl] = useState("http://application-load-balancer-1846011889.us-east-2.elb.amazonaws.com");

    const [switchValue, setSwitchValue] = useState(false);
    const [password, setPassword] = useState("");
    const [newUserName, setNewUserName] = useState("");
    const [newName, setNewName] = useState("");
    const [newFoto, setNewFoto] = useState("");
    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccess = (causa, mensaje) => {
        toast.current.show({ severity: "success", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccessMessage = (msg) => {
        message.current.show({ severity: "success", content: msg });
    };

    const showErrorMessage = (mensaje) => {
        message.current.show({ severity: "error", content: mensaje });
    };

    const handleInputSwitchChange = (e) => {
        setSwitchValue(e.value);
    };

    const checkPassword = () => {
        if (password === "") {
            showErrorMessage("¡Debe ingresar su contraseña para verificarla!");
        } else {
            showSuccessMessage("Validando contraseña...");
            saveChanges();
        }
    };

    /**
     * Procesa los archivos seleccionados previamente, los convierte en un <String> base 64 para
     * ser guardado en la base de datos
     * @param {*} event : Evento accionado al 'cargar' y 'subir' archivos de fotos seleccionadas
     */
    const onUpload = async ({ files }) => {
        let stringFoto = "";

        const [file] = files;
        const fileReader = new FileReader();
        fileReader.readAsDataURL(file);
        fileReader.onload = (e) => {
            let foto = e.target.result;
            stringFoto = foto.split(",")[1];
            updateProfileFoto(stringFoto);
            let base64 = fileReader.result;
            setNewFoto(base64.split(",")[1]);
        };
    };

    const updateProfileFoto = (foto) => {
        if (foto != "") {
            setNewFoto(foto);
            showSuccess("Carga Imagen", "La imagen se ha procesado y cargado correcatamente");
        }
    };

    const saveChanges = () => {
        console.dir(userData);
        let body = {
            _id: userData._id,
            username: newUserName !== "" ? newUserName : userData.username,
            name: newName !== "" ? newName : userData.name,
            password: password,
            photo: newFoto !== "" ? newFoto : "",
        };
        console.dir(body);
        authService.updateUser(body).then(data => {
            showSuccess("Modificacion Perfil", "La modificacion del perfil ha sido exitosa, cambios guardados");
            getNewUserData();
            setSwitchValue(false);
        }).catch(err => {
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + err.response.data);
        })

    };

    const getNewUserData = () => {
        console.log({ newUserName, password });
        let body = {
            username: newUserName !== "" ? newUserName : userData.username,
            password: password,
        };
        authService
            .login(body)
            .then((res) => {
                console.dir(res.data);
                window.localStorage.setItem("key", JSON.stringify(res.data.data));
                setUserData(res.data.data)
            })
            .catch((err) => {
                console.log(err.response);
                showWarn("Login Usuario", "Ha ocurrido un error al hacer la peticion: " + err.response.data.error);
            });
    };

    useEffect(() => {
        setNewUserName("");
        setNewName("");
        setNewFoto("");
    }, [userData]);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <h2>Mi informacion</h2>
                    <Toast ref={toast} />
                    <div className="grid p-fluid">
                        <div className="col-12">

                            <div className="card">
                                <h5>Tu foto de Perfil</h5>
                                <div className="flex justify-content-center">
                                    <Image src={userData.photo !== undefined ? userData.photo : "https://cdn.donmai.us/sample/d5/8b/__levi_shingeki_no_kyojin_drawn_by_neri_aot__sample-d58b3a3201254cb6e122bb2b0734180d.jpg"} alt="foto" width={450} preview />
                                </div>
                            </div>
                            <h5>Editar Informacion de Perfil</h5>
                            <InputSwitch checked={switchValue} onChange={handleInputSwitchChange} />
                            <br></br>
                            <br></br>
                            {switchValue ? (
                                <div className="card">
                                    <h5>Escoge tu archivo</h5>
                                    <Message severity={"info"} text={msgDesc} />
                                    <FileUpload customUpload uploadHandler={onUpload} onUpload={onUpload} multiple accept="image/*" maxFileSize={1000000} />
                                </div>
                            ) : (
                                <div />
                            )}
                        </div>
                        <div className="card">
                            <h5>Tu nombre de Usuario</h5>
                            <span className="p-float-label">
                                <InputText onChange={(e) => setNewUserName(e.target.value)} id="user" value={newUserName} type="text" disabled={!switchValue}></InputText>
                                <label htmlFor="user">{userData.username !== undefined ? userData.username : "No user-name"}</label>
                            </span>
                            <h5>Tu Nombre</h5>
                            <span className="p-float-label">
                                <InputText onChange={(e) => setNewName(e.target.value)} id="name" value={newName} type="text" disabled={!switchValue}></InputText>
                                <label htmlFor="name">{userData.name !== undefined ? userData.name : "No name"}</label>
                            </span>
                        </div>
                        {switchValue ? (
                            <div className="col-12">
                                <div className="col-12 md:col-6">
                                    <h5>Confirmar Contraseña y Guardar Cambios</h5>
                                    <div className="p-inputgroup">
                                        <InputText type="password" placeholder="Password" disabled={!switchValue} onChange={(e) => setPassword(e.target.value)} />
                                        <Button onClick={checkPassword} label="Guardar" />
                                    </div>
                                    <Messages ref={message} />
                                </div>
                            </div>
                        ) : (
                            <div />
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default InicioPerfil;
