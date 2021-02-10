const http = require('http');
const querystring = require('querystring');
const express = require('express');
const Web3 = require('web3');
const BigNumber = require('bignumber.js');
const ptimeout = require('promise.timeout')
const contract01 = require('./contract/contract.js');

const app = express();
const web3 = new Web3();
const eth = web3.eth;

web3.setProvider(new web3.providers.HttpProvider('http://163.17.137.169:6345'));
const myContract = web3.eth.contract(contract.ABI).at(contract.address);

var addressNum = ["0xe805cd2803eea1e65ad50d2efbab0c78a64ba868", "0xc2211d07db737e4746ffd0495024fda60b9b3c20", "0x3dc826e4ddd067657a68721068536f6af7ebc93e",
                  "0xfe506f205168d8b975c737ef222a521986bffdd9", "0xd40dee05018533da13e4d0006e175a25c594fbc7", "0xe08e1e210f42c5383137b7052f9addd50361913b",
				  "0x41f636295c566dad3a4ec1a70f1c29221ae6590c", "0x1e75d75adcb617789eede0b4bd32590e2217acbd", "0x752af796a8dd0c858dc8a65d45a5b89ff6b3584b",
				  "0xb510199eeabac2c99daf78fcf2106a2fae0f3e32", "0x65a03828f204dcf0d9b5c65fdbc9ff5a30a770ec", "0x95c83ca1eebfa1c21af999251d01154efa2db1b6",
				  "0xaa9fa09549d68d6b42223b0b5fc8bcc27fa262af", "0x7f5f45487f2c78877e51edd9e50d0f441bbe5f25", "0x02a58d976fe9cc556f929e35d08831518b27985c",
				  "0x123ac604e24b69ce6dca752b1c86b445f8555dbe", "0x907b4a61c6d0a4c41dff3c203884524102dadd42", "0xda48f645c02f6e1593d9e942023241ca1f882b5e",
				  "0xb44993dc71b986bc2cc3e9ade0a7ad62e64b73a8", "0xe6e94ed8948d43a0df6a05dc608ea115f4266efb"];
var addressNumStatus = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
var dealstatus = 0;
var dealTime = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""];
var saveAllTimeString = "";
/*---------------------------------------------------------------------------*/
/*---------------------------------------------------------------------------*/
app.post('/ListenEvent', function(req, res) {
	let body = "";
    req.on('data', function (chunk) {
        body += chunk;
    });
    req.on('end', async function () {
        body = JSON.parse(body); 	
	    if(body["useraddress"]) { 
			let blockPromise = new Promise((resole, reject) => {
				web3.eth.getBlockNumber(function (error, result) {
                    if(!error){
						resole(result);
                    }
                });
			});
			let saveBlockPromise = await blockPromise.then(function(data){
				return data;
			});
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "0",
					"blockHash": saveBlockPromise,
					"dealStatus": dealstatus
				} 
			};
			res.json(obj);
		}
        res.end();
    });
	return;
});
/*---------------------------------------------------------------------------*/
app.post('/Transform', function(req, res) {
	let body = "";
    req.on('data', function (chunk) {
        body += chunk;
    });
    req.on('end', async function () {
        body = JSON.parse(body); 	
		if(body["status"] == "BalanceOf"){
			if(body["useraddress"]) { 
				let balanceOfValue = await myContract.balanceOf.call(body["useraddress"]);
				let balanceOfBignumber = new BigNumber(balanceOfValue);
				let obj ={
					"ResultCode": "0",
					"Data": {
						"callnumber": "2",
						"value": balanceOfBignumber.plus(0).toString(10)
					}
				};
				res.json(obj);
			}    	
			res.end();
		}
		else if(body["status"] == "TwoPointDeal"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "3",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["sendtoaddress"] && body["sendamount"]) { 
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], "", 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let approvePromise = new Promise((resole, reject) => {
						myContract.transfer.sendTransaction(body["sendtoaddress"], body["sendamount"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveApprovePromise = await approvePromise.then(function(data){
						return data;
					});
					obj.Data.value = saveApprovePromise;
				}
			}  	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "GetAddress"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "4",
					"value": "",
					"number": ""
				}
			};
			let controllAddressNum = 0;
			for(let i = 0;i <= 19; i++){
				if(controllAddressNum == 0){
					if(addressNumStatus[i] == 0){
						obj.Data.value = addressNum[i];
						obj.Data.number = i;
						addressNumStatus[i] = 1;
						controllAddressNum = 1;
					}
				}
			}
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "GetOtherAddress"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "5",
					"value": "",
					"mynumber": ""
				}
			};
			let saveAddressNum;
			for(let i = 0;i <= 19; i++){
				if(addressNum[i] == body["address"]){
					saveAddressNum = i;
				}
			}
			
			
			obj.Data.value = addressNum[saveAddressNum+1];
			obj.Data.mynumber = saveAddressNum;
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ResetAddress"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "6",
					"value": ""
				}
			};
			for(let i = 0;i <= 19; i++){
				addressNumStatus[i] = 0;
			}
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ChangeDealStatus0"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "7",
					"value": dealstatus
				}
			};
			dealstatus = 0;
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ChangeDealStatus1"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "7",
					"value": dealstatus
				}
			};
			dealstatus = 1;
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "GetDealTime"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "8",
					"value": ""
				}
			};
			let addressNum = body["otherADNum"];
			dealTime[addressNum] = body["dealTime"];
			console.log(addressNum);
			console.log(dealTime[addressNum]);
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ShowAllDealTime"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "9",
					"value": ""
				}
			};
			let checkNull = 0;
			if(checkNull == 0){
				for(let i = 0;i <= 19; i++){
					saveAllTimeString += "第" + i + "個 : " + dealTime[i] + " ";
				}
			}
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "GetAllDealTime"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "10",
					"value": saveAllTimeString
				}
			};
			res.json(obj);
			res.end();
		}
    });
	return;
});
/*---------------------------------------------------------------------------*/
/*---------------------------------------------------------------------------*/

app.listen(3000, function () {
  console.log('Example app is running on port 3000!');}
);

/*---------------------------------------------------------------------------*/
/*---------------------------------------------------------------------------*/