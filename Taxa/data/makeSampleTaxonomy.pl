#!/usr/bin/env perl

use strict;
use warnings;
use Data::Dumper;

my %parent;
my %child;
my %name;
my %node;
open(my $fh, "nodes.dmp"); 
while(<$fh>){ 
  chomp;
  my($taxid,$parentid)=split(/\t\|\t/);
  $parent{$taxid}=$parentid; 
  push(@{ $child{$parentid} }, $taxid);
  $node{$taxid}=$_;
} 
close $fh; 

open(my $namesfh, "names.dmp"); 
while(<$namesfh>){
  chomp; 
  my($taxid,$name,$unique,$class)=split(/\t\|\t/); 
  push(@{$name{$taxid}}, $_);
} 
close $namesfh; 

my $lcaTaxid="666";
my @nodesToPrint;
addChildren($lcaTaxid,\@nodesToPrint);

open(my $outnodes, ">", "nodes.vibrio.dmp");
open(my $outnames, ">", "names.vibrio.dmp"); 
for my $taxid(@nodesToPrint){
  print $outnodes $node{$taxid}."\n";
  for my $nameEntry(@{ $name{$taxid} }){
    print $outnames $nameEntry."\n";
  }
}


sub addChildren{
  my($taxid,$nodes)=@_;
  push(@$nodes, $taxid);

  if(defined($child{$taxid})){
    for my $child(@{ $child{$taxid} }){
      addChildren($child,$nodes);
    }
  }
}
