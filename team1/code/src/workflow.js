const _ = require('lodash');
const fs = require('fs');
const axios = require('axios');

const cdaXmlPath = './hl7CDA/';
const cdaFile = 'cda1';

const fdnsStorageApi = 'http://storage.datalake.services-dev.cdc.gov/api/1.0/';
const fdnsCdaApi = 'http://cda.fx.services-dev.cdc.gov/api/1.0/';
const fdnsRulesApi = 'http://rules.fx.services-dev.cdc.gov/api/1.0/';
const fndsObjectApi = 'http://object.datalake.services-dev.cdc.gov/api/1.0/';
const fndsIndexApi = 'http://indexing.datalake.services-dev.cdc.gov/api/1.0/';

const storageDrawer = 'team1a';



const drawerExists = async () => {
    try {
        await axios.get(`${fdnsStorageApi}drawer/${storageDrawer}`);
        return true;
    } catch(err) {
        return false;
    };
}

const createDrawer = async () => {
    await axios.put(`${fdnsStorageApi}drawer/${storageDrawer}`);
    console.log('drawer created');
}

drawerExists().then(result => {
    if (result) {
        console.log('drawer already exists');
    }
    else {async () => {
        await createDrawer();
    }
} 
});

// get the cda file (xml) to store into the immutable data store (data lake)
const cdaXml = fs.readFileSync(`${cdaXmlPath + cdaFile}.xml`, 'utf8');
console.log('read xml file from disk');
// console.log(cdaXml)

    
// add file to drawer, replace if already exists

// curl -X POST "http://storage.datalake.services-dev.cdc.gov/api/1.0/node/team1a?generateId=true&generateStruct=true&id=cda4.xml&replace=false" -H "accept: application/json" -H "Content-Type: multipart/form-data" -F "file=@cda4.xml;type=text/xml"



const addFileToDrawer = async () => await axios.post(`${fdnsStorageApi}node/team1a?generateId=true&generateStruct=true&id=${cdaFile}.xml&replace=true1`, cdaXml);

addFileToDrawer().then(console.log('file added to drawer.'))



// const nodeExistsInDrawer = async () => {
//     const resp = await axios.get(`${fdnsStorageApi}drawer/nodes/${storageDrawer}`);
//     console.log(resp.data);
// }

// nodeExistsInDrawer();

//     const resp = axios.get(`${fdnsStorageApi}drawer/${storageDrawer}`)

//     .then(resp => {
//         console.log(`Drawer ${storageDrawer} already created`);
//     })
//     .catch(err => {
//         // create drawer
//         axios.put(`${fdnsStorageApi}drawer/${storageDrawer}`)
//             .then(resp => {
//                 console.log(`Drawer ${storageDrawer} created`);
//             });
//             // exit on error
//     });



// // file in drawer?
// axios.get(`${fdnsStorageApi}drawer/nodes/${storageDrawer}`)
//     .then(resp => {
//         console.log(resp);
//     });


