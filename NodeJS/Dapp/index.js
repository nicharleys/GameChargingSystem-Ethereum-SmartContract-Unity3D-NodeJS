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
					"callnumber": "2",
					"blockHash": saveBlockPromise
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
		if(body["status"] == "Allowance"){
			if(body["useraddress"] && body["otheruseraddress"]) { 
				let allowanceValue = await myContract.allowance.call(body["useraddress"], body["otheruseraddress"]);
				let allowanceBignumber = new BigNumber(allowanceValue);
				let obj ={
					"ResultCode": "0",
					"Data": {
						"callnumber": "3",
						"value": allowanceBignumber.plus(0).toString(10)
					}
				};
				res.json(obj);
			}    	
			res.end();
		}
		else if(body["status"] == "BalanceOf"){
			if(body["useraddress"]) { 
				let balanceOfValue = await myContract.balanceOf.call(body["useraddress"]);
				let balanceOfBignumber = new BigNumber(balanceOfValue);
				let obj ={
					"ResultCode": "0",
					"Data": {
						"callnumber": "4",
						"value": balanceOfBignumber.plus(0).toString(10)
					}
				};
				res.json(obj);
			}    	
			res.end();
		}
		else if(body["status"] == "LookExchange_Self"){
			if(body["useraddress"] && body["otheruseraddress"]) { 
				let lookExchangeItem = await myContract.lookExchange.call(body["useraddress"], body["otheruseraddress"]);
				let balancesValue = new BigNumber(lookExchangeItem[0]);
				let item1Value = new BigNumber(lookExchangeItem[1]);
				let item2Value = new BigNumber(lookExchangeItem[2]);
				let statusValue = new BigNumber(lookExchangeItem[3]);
				let obj ={
					"ResultCode": "0",
					"Data": {
						"callnumber": "8",
						"balances": balancesValue,
						"item1": item1Value,
						"item2": item2Value,
						"status": statusValue
					}
				};
				res.json(obj);
			}    	
			res.end();
		}
		else if(body["status"] == "LookExchange_Other"){
			if(body["useraddress"] && body["otheruseraddress"]) { 
				let lookExchangeItem = await myContract.lookExchange.call(body["otheruseraddress"], body["useraddress"]);
				let balancesValue = new BigNumber(lookExchangeItem[0]);
				let item1Value = new BigNumber(lookExchangeItem[1]);
				let item2Value = new BigNumber(lookExchangeItem[2]);
				let statusValue = new BigNumber(lookExchangeItem[3]);
				let obj ={
					"ResultCode": "0",
					"Data": {
						"callnumber": "9",
						"balances": balancesValue,
						"item1": item1Value,
						"item2": item2Value,
						"status": statusValue
					}
				};
				res.json(obj);
			}    	
			res.end();
		}
		else if(body["status"] == "LookLottery"){
			if(body["LotteryStatus"] == "1") { 
				let lotteryStatus = {
					"ResultCode": "0",
					"Data": {
						"callnumber": "14",
						"getlotteryOwnerreturn": myContract.lotteryOwner.call(),
						"getlookMoneyreturn": myContract.lookMoney.call(),
						"getlookPlayersreturn": myContract.players.call(body["playerNumbers"], body["useraddress"])
					}
				};
				res.json(lotteryStatus);
			} 
			res.end();
		}
		else if(body["status"] == "UnlockAccount"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "0",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"]) { 
				await web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
				});
			}  	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ContractStatus"){
			if(body["ContractStatus"] == "1") { 
				let contractreturn1 = {
					"ResultCode": "0",
					"Data": {
						"callnumber": "1",
						"getnamereturn": myContract.name.call(),
						"getownerreturn": myContract.owner.call()
					}
				};
				res.json(contractreturn1);
			} 
			res.end();
		}
        else if(body["status"] == "GiveApprove"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "5",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"] && body["sendamount"]) { 
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let approvePromise = new Promise((resole, reject) => {
						myContract.approve.sendTransaction(body["sendtoaddress"], body["sendamount"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveApprovePromise = await approvePromise.then(function(data){
						return data;
					});
				}
			} 		
			res.json(obj);
			res.end();
		}
        else if(body["status"] == "TwoPointDeal"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "6",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"] && body["sendamount"]) { 
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
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
				}
			}  	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "PutExchange"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "7",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"] && body["balances"] && body["item1"] && body["item2"]){
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let putExchangePromise = new Promise((resole, reject) => {
						myContract.putExchange.sendTransaction(body["sendtoaddress"], body["balances"], body["item1"], body["item2"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let savePutExchangePromise = await putExchangePromise.then(function(data){
						return data;
					});
				}
			}	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ExchangeStatus"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "10",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"]){
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let exchangeStatusPromise = new Promise((resole, reject) => {
						myContract.exchangeStatus.sendTransaction(body["sendtoaddress"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveExchangeStatusPromise = await exchangeStatusPromise.then(function(data){
						return data;
					});
				}
			}	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "CancelExchange"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "11",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"]){
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let cancelExchangePromise = new Promise((resole, reject) => {
						myContract.cancelExchange.sendTransaction(body["sendtoaddress"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveCancelExchangePromise = await cancelExchangePromise.then(function(data){
						return data;
						
					});
				}
			}	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "ExchangeItem"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "12",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendtoaddress"]){
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let exchangeItemPromise = new Promise((resole, reject) => {
						myContract.exchangeItem.sendTransaction(body["sendtoaddress"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveExchangeItemPromise = await exchangeItemPromise.then(function(data){
						return data;
					});
				}
			}	
			res.json(obj);
			res.end();
		}
		else if(body["status"] == "BuyLottery"){
			let obj ={
				"ResultCode": "0",
				"Data": {
					"callnumber": "13",
					"value": "1"
				}
			};
			if(body["useraddress"] && body["userpassword"] && body["sendamount"]){
				let unlockAccountPromise = new Promise((resole, reject) => {
					web3.personal.unlockAccount(body["useraddress"], body["userpassword"], 10, (error, result) => {
						resole(result);
					});
				});
				let saveUnlockAccountPromise = await unlockAccountPromise.then(function(data){
					return data;
				});
				if(saveUnlockAccountPromise == true){
					let buyLotteryPromise = new Promise((resole, reject) => {
						myContract.enter.sendTransaction(body["sendamount"], {from: body["useraddress"], gas: "300000"}, (error, result) => {
							resole(result);
						});
					});
					let saveBuyLotteryPromise = await buyLotteryPromise.then(function(data){
						return data;
					});
				}
			}	
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