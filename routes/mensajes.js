import fs from 'fs';
import express from 'express';
import { microservicioMultimedia } from '../clients/http/multimedia.js';
import { microservicioMensaje } from '../clients/http/mensaje.js';
import { microservicioTypers } from '../clients/http/typer.js';

var folderPath = process.cwd() + "/archivosTemporales/";
fs.mkdir(folderPath, null, function (err) {
    if (err) {
        console.log(err);
    }
})

const router = express.Router();

router.post("/registrarMultimedia", async (req, res) => {
    var idTyper = req.query.idTyper;
    var { file }  = req.files;
    var filePath = folderPath + file.name;

    fs.writeFile(filePath, file.data, function (err) {
        if (err) {
            res.send(err);
        }else{
            microservicioMultimedia.RegistrarMultimedia(filePath, idTyper)
            .then(response => {
                let resultado = {
                    status: response.status,
                    message: response.data.message,
                    result: response.data.result
                }
                res.send(resultado)
            })
            .catch(error => {
                res.send(error);
            }).finally(() => {
                fs.unlink(filePath, function (err) {
                    if (err) console.log(err);
                });
            })
        }
    })
})



router.get("/obtenerMultimedia", async (req, res) => {
    const { idMultimedia } = req.query;
    
    res.send(
        {
            'urlMultimedia': microservicioMultimedia.obtenerMultimedia(idMultimedia)
        });
})

router.get("/obtenerGrupos", async (req, res) => {
    const { nombre, idGrupo} = req.query;

    microservicioMensaje.ObtenerGrupos(nombre, idGrupo)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

router.get("/integrantesDeGrupo/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;

    if (idGrupo > 0)
    {
        let response = await microservicioMensaje.ObtenerIntegrantesDeGrupo(idGrupo)

        if (response.status == true)
        {
            let typers = [];
            let integrantesDeGrupo = response.data.result;

            typers = await Promise.all(
                integrantesDeGrupo.map((integrante) =>
                    microservicioTypers.ObtenerTyperPorId(integrante.IdTyper)
                )
            )

            let resultado = {
                status: true,
                message: 'Integrantes encontrados',
                result: typers
            };
        
            res.send(resultado)
        }
        else
        {
            res.send(response)
        }
    }
})

router.post("/crearGrupo", async (req, res) => {
    let nuevoGrupo = req.body;

    microservicioMensaje.CrearGrupo(nuevoGrupo)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/actualizarGrupo/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;
    let grupoActualizado = req.body;

    microservicioMensaje.ActualizarGrupo(idGrupo, grupoActualizado)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

router.post("/agregarIntegrantes/:idGrupo", async (req, res) => {
    let idGrupo = parseInt(req.params.idGrupo) || -1;
    let integrantesBody = req.body;

    let nuevosIntegrantes = []

    integrantesBody.forEach(integrante => {
        nuevosIntegrantes.push({
            idGrupo: idGrupo,
            idTyper: integrante.IdTyper
        })
    });

    microservicioMensaje.AgregarIntegrantesAGrupo(idGrupo, nuevosIntegrantes)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/salirDeGrupo", async (req, res) => {
    const idGrupo = req.query.idGrupo || -1;
    const idTyper = req.query.idTyper || "";

    microservicioMensaje.SalirDeGrupo(idGrupo, idTyper)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
    
})

router.post("/enviarMensaje", async (req, res) => {
    const nuevoMensajeBody = req.body

    let nuevoMensaje = {
        contenido: nuevoMensajeBody.contenido,
        idGrupo: nuevoMensajeBody.idGrupo,
        idTyper: nuevoMensajeBody.idTyper,
        idMultimedia: nuevoMensajeBody.idMultimedia
    }

    microservicioMensaje.EnviarMensaje(nuevoMensaje)
    .then((response) => {
        microservicioTypers.ObtenerTyperPorId(nuevoMensaje.idTyper)
        .then(typer => {
            let resultado = {}

            if (nuevoMensajeBody.idMultimedia){
                resultado = {
                    status: true,
                    message: "Mensaje enviado",
                    result : {
                        idMensaje: response.data.result.IdMensaje,
                        contenido: nuevoMensajeBody.contenido,
                        fecha: response.data.result.Fecha,
                        hora: response.data.result.Hora,
                        idGrupo: response.data.result.IdGrupo,
                        typer: typer,
                        idMultimedia: microservicioMultimedia.obtenerMultimedia(nuevoMensajeBody.idMultimedia)
                    }
                };
            }else{
                resultado = {
                    status: true,
                    message: "Mensaje enviado",
                    result : {
                        idMensaje: response.data.result.IdMensaje,
                        contenido: nuevoMensajeBody.contenido,
                        fecha: response.data.result.Fecha,
                        hora: response.data.result.Hora,
                        idGrupo: response.data.result.IdGrupo,
                        typer: typer,
                        idMultimedia: ""
                    }
                }
            }

            res.send(resultado)
        })
        .catch(error => {
            res.send(error)
        })
    })
    .catch(error => {
        res.send(error)
    })
})

router.get("/obtenerMensajes/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;

    let response = await microservicioMensaje.ObtenerMensajesDeGrupo(idGrupo);

    if (response.status === true) {
        Promise.all(
            response.data.result.map(async (mensaje) => {
                return microservicioTypers.ObtenerTyperPorId(mensaje.IdTyper)
                .then(typer => {
                    if (typer) {
                        if (mensaje.IdMultimedia) {
                            return {
                                idMensaje: mensaje.IdMensaje,
                                contenido: mensaje.Contenido,
                                fecha: mensaje.Fecha,
                                hora: mensaje.Hora,
                                idGrupo: mensaje.IdGrupo,
                                typer: typer,
                                idMultimedia: microservicioMultimedia.obtenerMultimedia(mensaje.IdMultimedia)
                            };
                        } else {
                            return {
                                idMensaje: mensaje.IdMensaje,
                                contenido: mensaje.Contenido,
                                fecha: mensaje.Fecha,
                                hora: mensaje.Hora,
                                idGrupo: mensaje.IdGrupo,
                                typer: typer,
                                idMultimedia: "",
                            };
                        }
                    }
                    
                })
            })
        ).then(resultado => res.send({
            status: true,
            message: "Mensajes encontrados",
            result: resultado
        }));   
    }
    else
    {
        res.send(response)
    }
})

router.get("/misGrupos/:idTyper", async (req, res) => {
    let idTyper = req.params.idTyper || "";

    microservicioMensaje.ObtenerMisGrupos(idTyper)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }

        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

export default router;