<?php

namespace App\Http\Controllers;

use Mail;
use App\Http\Controllers\Controller;
use App\Http\Requests;

use Illuminate\Http\Request;


class MailsController extends Controller{

    public function __construct()
    {
    }

    /**
     * Envia o email para uma lista de emails.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        // $this->validate($request, [
        //     'name' => 'required',
        //     'email' => 'required|email',
        // ]);

        Mail::send('mails.index', ['mail' => $request->only('subject','message')], function($message) use ($request) {
            $message->from(config('mail.from.address'), config('mail.from.name'));
            foreach ($request->users as $email) {
                $message->to($email['email']);
            }
            $message->subject($request->subject);
        });

        $failure = array();
        if(count(Mail::failures()) > 0){
            foreach (Mail::failures as $emails) {
               array_push($failure, $emails);
            }
        }

        return $failure;
    }

}

?>
