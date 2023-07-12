import React, { useState, useRef, useEffect } from "react";
import { Message } from "primereact/message";
import { Toast } from "primereact/toast";
import { InputTextarea } from "primereact/inputtextarea";
import { Button } from "primereact/button";
import { FileUpload } from "primereact/fileupload";
import { AuthService } from "../service/AuthService";

const CrearPublicacion = () => {
    const toast = useRef();
    const msgDesc = "Descripcion de imagen seleccionada (Esta campo es obligatorio)";

    const [uploadedImage, setUploadedImage] = useState("");
    const [textImageDescription, setTextImageDescription] = useState("");
    const [totalSize, setTotalSize] = useState(0);
    const [buttonDisabled, setButtonAsDisabled] = useState(true);
    const [spinnerIsVisible, setSpinnerIsVisible] = useState(false);

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    /**
     * Acciones que se realizan para crear y cargar nueva publicacion
     */
    const hancleOnClickAddPost = () => {
        console.log("Cargango Imagen...");
        let body = {
            username: userData.username,
            text: textImageDescription,
            image: fixImage(uploadedImage),
        };
        try {
            new AuthService().add_publication(body)
                .then((res) => {
                    console.log(res);
                    showNotification("Crear Publicacion", "¡La publicacion ha sido creada con exito!", "success");
                })
                .catch((err) => {
                    console.log(err.response);
                    showNotification("Crear Publicacion, API", "Algo ha ocurrido al cargar image:" + err.response.data, "warn");
                });
        } catch (error) {
            console.error(error);
        }
    };

    /**
     * Acciones a realizar cuando se cancela y remueven todos los archivos en el componente FileUpload
     */
    const onTemplateClear = () => {
        setUploadedImage("");
    };

    /**
     * Propiedad perteneciente a componente FileUpload, es la plantilla
     * para desplegar los elementos cargados (archivos)
     * @param {*} file
     * @param {*} props
     * @returns JSX Component
     */
    const itemTemplate = (file, props) => {
        return (
            <div className="flex align-items-center flex-wrap">
                <div className="flex align-items-center" style={{ width: "40%" }}>
                    <img alt={file.name} role="presentation" src={file.objectURL} width={300} />
                    <span className="flex flex-column text-left ml-3">
                        {file.name}
                        <small>{new Date().toLocaleDateString()}</small>
                    </span>
                </div>
                <Button type="button" icon="pi pi-times" className="p-button-outlined p-button-rounded p-button-danger ml-auto" onClick={() => onTemplateRemove(file, props.onRemove)} />
            </div>
        );
    };

    /**
     * Muestra una notificacion
     * @param {string} causa : (String) encabezado que acciona la notificacion
     * @param {string} mensaje : (String) mensaje que se mostrara en la notificacion
     * @param {string} severity : (String) severidad (info|warn|error|sucess)
     */
    const showNotification = (causa, mensaje, severity = "info") => {
        toast.current.show({ severity: severity, summary: causa, detail: mensaje, life: 6000 });
    };

    /**
     * arregal el texto URL-base64 y lo convierte en texto puro de la imagen en base-64
     * @param {string} image : texto URL-base64
     * @returns {string} cadena 64-base
     */
    const fixImage = (image) => {
        return image.split(",")[1];
    };

    /**
     * Acciones a relizar cuando un archivo es removido de la lista del componenete
     * @param {*} file
     * @param {*} callback
     */
    const onTemplateRemove = (file, callback) => {
        setUploadedImage("");
        setTotalSize(totalSize - file.size);
        callback();
    };

    /**
     * Obtiene el archivo recien cargado y lo convierte en una imagen de texto 64-base
     * @param {*} param0 : files from event
     */
    const onUpload = async ({ files }) => {
        const [file] = files;
        const fileReader = new FileReader();
        fileReader.readAsDataURL(file);
        fileReader.onload = (e) => {
            let foto = e.target.result;
            setUploadedImage(foto);
            showNotification("Add Image", "La imagen ha sido cargada y procesada correctamente", "success");
        };
    };

    useEffect(() => {
        if ((textImageDescription === "") | (uploadedImage === "")) {
            setButtonAsDisabled(true);
        } else {
            setButtonAsDisabled(false);
        }
    }, [textImageDescription]);

    useEffect(() => {
        if ((uploadedImage === "") | (textImageDescription === "")) {
            setButtonAsDisabled(true);
        } else {
            setButtonAsDisabled(false);
        }
    }, [uploadedImage]);

    return (
        <div className="gird">
            <div className="col-12">
                <div className="card">
                    <Toast ref={toast} />
                    <h2>Crear Publicación</h2>
                    <div>
                        <div className="col-12">
                            <h5>Escoge tu archivo</h5>
                            <Message severity="info" text="Despues de seleccionar tu archivo debes hacer clik en 'Upload' para procesar y cargar la imagen." />
                            <FileUpload customUpload onClear={onTemplateClear} uploadHandler={onUpload} itemTemplate={itemTemplate} onUpload={onUpload} accept="image/*" />
                            <Message severity={"info"} text={msgDesc} />
                            <InputTextarea value={textImageDescription} onChange={(e) => setTextImageDescription(e.target.value)} rows={5} cols={47} />
                        </div>
                        <div className="col-20">
                            <div className="">
                                <Button onClick={hancleOnClickAddPost} label="Cargar imagen" icon="pi pi-bolt" disabled={buttonDisabled} />
                            </div>
                            {spinnerIsVisible ? (
                                <>
                                    <div className="col-12">
                                        <i className="pi pi-spin pi-spinner" style={{ fontSize: "2em" }}></i>
                                    </div>
                                    <p>Cargando...</p>
                                </>
                            ) : (
                                <></>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CrearPublicacion;
